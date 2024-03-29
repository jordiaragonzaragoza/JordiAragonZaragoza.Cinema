namespace JordiAragon.Cinema.Reservation.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Rules;
    using JordiAragon.Cinema.Reservation.User.Domain;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using JordiAragon.SharedKernel.Domain.Services;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public class ShowtimeManager : BaseDomainService, IShowtimeManager
    {
        public static readonly TimeSpan CleaningAndAccessTimeSpan = TimeSpan.FromMinutes(30);

        private readonly IReadRepository<Movie, MovieId> movieRepository;
        private readonly IReadRepository<Auditorium, AuditoriumId> auditoriumRepository;

        public ShowtimeManager(
            IReadRepository<Movie, MovieId> movieRepository,
            IReadRepository<Auditorium, AuditoriumId> auditoriumRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public static IEnumerable<Seat> AvailableSeats(Auditorium auditorium, Showtime showtime)
        {
            var reservedSeats = ReservedSeats(auditorium, showtime);

            return auditorium.Seats.Except(reservedSeats)
                                        .OrderBy(s => s.Row)
                                        .ThenBy(s => s.SeatNumber);
        }

        public async Task<Ticket> ReserveSeatsAsync(
            Showtime showtime,
            IEnumerable<SeatId> desiredSeatIds,
            TicketId newTicketId,
            UserId userId,
            DateTimeOffset currentDateTimeOnUtc,
            CancellationToken cancellationToken)
        {
            Guard.Against.Null(showtime);
            Guard.Against.NullOrEmpty(desiredSeatIds);
            Guard.Against.Null(newTicketId);
            Guard.Against.Null(userId);
            Guard.Against.Default(currentDateTimeOnUtc);

            var existingMovie = await this.movieRepository.GetByIdAsync(showtime.MovieId, cancellationToken);
            if (existingMovie is null)
            {
                throw new NotFoundException(nameof(Movie), showtime.MovieId.ToString());
            }

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(showtime.AuditoriumId, cancellationToken);
            if (existingAuditorium is null)
            {
                throw new NotFoundException(nameof(Auditorium), showtime.AuditoriumId.ToString());
            }

            CheckRule(new NoReservationsAllowedAfterShowtimeEndedRule(showtime, existingMovie, currentDateTimeOnUtc));

            var desiredSeats = existingAuditorium.Seats.Where(seat => desiredSeatIds.Contains(seat.Id));

            CheckRule(new OnlyContiguousSeatsCanBeReservedRule(desiredSeats));

            var availableSeats = AvailableSeats(existingAuditorium, showtime);

            CheckRule(new OnlyAvailableSeatsCanBeReservedRule(desiredSeats, availableSeats));

            return showtime.ReserveSeats(newTicketId, userId, desiredSeatIds, currentDateTimeOnUtc);
        }

        public async Task<bool> HasShowtimeEndedAsync(Showtime showtime, DateTimeOffset currentDateTimeOnUtc, CancellationToken cancellationToken)
        {
            Guard.Against.Null(showtime);
            Guard.Against.Default(currentDateTimeOnUtc);

            var existingMovie = await this.movieRepository.GetByIdAsync(showtime.MovieId, cancellationToken);
            if (existingMovie is null)
            {
                throw new NotFoundException(nameof(Movie), showtime.MovieId.ToString());
            }

            var rule = new NoReservationsAllowedAfterShowtimeEndedRule(showtime, existingMovie, currentDateTimeOnUtc);
            if (rule.IsBroken())
            {
                return true;
            }

            return false;
        }

        private static IEnumerable<Seat> ReservedSeats(Auditorium auditorium, Showtime showtime)
        {
            var seatIds = showtime.Tickets.SelectMany(ticket => ticket.Seats);

            return auditorium.Seats.Where(seat => seatIds.Contains(seat.Id))
                                        .OrderBy(s => s.Row)
                                        .ThenBy(s => s.SeatNumber);
        }
    }
}