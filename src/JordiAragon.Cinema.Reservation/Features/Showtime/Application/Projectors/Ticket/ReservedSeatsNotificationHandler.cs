namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projectors.Ticket
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using MediatR;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ReservedSeatsNotificationHandler : INotificationHandler<ReservedSeatsNotification>
    {
        private readonly IReadRepository<Showtime, ShowtimeId> showtimeRepository;
        private readonly IReadRepository<Auditorium, AuditoriumId> auditoriumRepository;
        private readonly IReadRepository<Movie, MovieId> movieRepository;
        private readonly IRepository<TicketReadModel, Guid> ticketReadModelRepository;

        public ReservedSeatsNotificationHandler(
            IReadRepository<Showtime, ShowtimeId> showtimeRepository,
            IReadRepository<Auditorium, AuditoriumId> auditoriumRepository,
            IReadRepository<Movie, MovieId> movieRepository,
            IRepository<TicketReadModel, Guid> ticketReadModelRepository)
        {
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
            this.ticketReadModelRepository = Guard.Against.Null(ticketReadModelRepository, nameof(ticketReadModelRepository));
        }

        public async Task Handle(ReservedSeatsNotification notification, CancellationToken cancellationToken)
        {
            var @event = notification.Event;

            var existingShowtime = await this.showtimeRepository.GetByIdAsync(ShowtimeId.Create(@event.ShowtimeId), cancellationToken);
            if (existingShowtime is null)
            {
                throw new NotFoundException(nameof(Showtime), @event.ShowtimeId.ToString());
            }

            var existingMovie = await this.movieRepository.GetByIdAsync(MovieId.Create(existingShowtime.MovieId), cancellationToken);
            if (existingMovie is null)
            {
                throw new NotFoundException(nameof(Movie), existingShowtime.MovieId.ToString());
            }

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(AuditoriumId.Create(existingShowtime.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                throw new NotFoundException(nameof(Auditorium), existingShowtime.AuditoriumId.ToString());
            }

            var seats = existingAuditorium.Seats.Where(seat => @event.SeatIds.Contains(seat.Id))
                .Select(x => new SeatReadModel(x.Id, x.Row, x.SeatNumber)).ToList();

            var ticketReadModel = new TicketReadModel(
                @event.TicketId,
                @event.UserId,
                @event.ShowtimeId,
                existingShowtime.SessionDateOnUtc,
                existingAuditorium.Name,
                existingMovie.Title,
                seats,
                false,
                @event.CreatedTimeOnUtc);

            await this.ticketReadModelRepository.AddAsync(ticketReadModel, cancellationToken);
        }
    }
}