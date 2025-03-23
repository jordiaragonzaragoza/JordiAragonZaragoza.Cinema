namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Rules;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using JordiAragonZaragoza.SharedKernel.Domain.Services;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ReservationManager : BaseDomainService, IReservationManager
    {
        public static readonly TimeSpan CleaningAndAccessTimeSpan = TimeSpan.FromMinutes(30);

        private readonly IReadRepository<Movie, MovieId> movieRepository;
        private readonly IReadRepository<Auditorium, AuditoriumId> auditoriumRepository;

        public ReservationManager(
            IReadRepository<Movie, MovieId> movieRepository,
            IReadRepository<Auditorium, AuditoriumId> auditoriumRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public static IEnumerable<Seat> AvailableSeats(Auditorium auditorium, Showtime showtime)
        {
            ArgumentNullException.ThrowIfNull(auditorium, nameof(auditorium));
            ArgumentNullException.ThrowIfNull(showtime, nameof(showtime));

            var reservedSeats = ReservedSeats(auditorium, showtime);

            return auditorium.Seats.Except(reservedSeats)
                                        .OrderBy(s => s.Row)
                                        .ThenBy(s => s.SeatNumber);
        }

        public async Task<Reservation> ReserveSeatsAsync(
            Showtime showtime,
            IEnumerable<SeatId> desiredSeatIds,
            ReservationId newReservationId,
            UserId userId,
            ReservationDate reservationDateOnUtc,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(showtime);
            Guard.Against.NullOrEmpty(desiredSeatIds);
            ArgumentNullException.ThrowIfNull(newReservationId);
            ArgumentNullException.ThrowIfNull(userId);
            Guard.Against.Default(reservationDateOnUtc);

            var existingMovie = await this.movieRepository.GetByIdAsync(showtime.MovieId, cancellationToken);
            if (existingMovie is null)
            {
                throw new NotFoundException(nameof(Movie), showtime.MovieId.ToString()!);
            }

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(showtime.AuditoriumId, cancellationToken);
            if (existingAuditorium is null)
            {
                throw new NotFoundException(nameof(Auditorium), showtime.AuditoriumId.ToString()!);
            }

            CheckRule(new NoReservationsAllowedAfterShowtimeEndedRule(showtime, existingMovie, reservationDateOnUtc));

            var desiredSeats = existingAuditorium.Seats.Where(seat => desiredSeatIds.Contains(seat.Id));

            CheckRule(new OnlyContiguousSeatsCanBeReservedRule(desiredSeats));

            var availableSeats = AvailableSeats(existingAuditorium, showtime);

            CheckRule(new OnlyAvailableSeatsCanBeReservedRule(desiredSeats, availableSeats));

            return showtime.ReserveSeats(newReservationId, userId, desiredSeatIds, reservationDateOnUtc);
        }

        // TODO: This method will gone out on using sagas with timeout messages to mark showtimes as ended.
        public async Task<bool> HasShowtimeEndedAsync(Showtime showtime, DateTimeOffset currentDateTimeOnUtc, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(showtime);
            Guard.Against.Default(currentDateTimeOnUtc);

            var existingMovie = await this.movieRepository.GetByIdAsync(showtime.MovieId, cancellationToken);
            if (existingMovie is null)
            {
                throw new NotFoundException(nameof(Movie), showtime.MovieId.ToString()!);
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
            var seatIds = showtime.Reservations.SelectMany(reservation => reservation.Seats);

            return auditorium.Seats.Where(seat => seatIds.Contains(seat.Id))
                                        .OrderBy(s => s.Row)
                                        .ThenBy(s => s.SeatNumber);
        }
    }
}