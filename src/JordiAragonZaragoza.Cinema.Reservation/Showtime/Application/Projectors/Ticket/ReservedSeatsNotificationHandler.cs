namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Ticket
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;

    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ReservedSeatsNotificationHandler : IEventNotificationHandler<ReservedSeatsNotification>
    {
        private readonly IReadRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository;
        private readonly IReadRepository<AuditoriumReadModel, Guid> auditoriumReadModelRepository;
        private readonly IReadRepository<MovieReadModel, Guid> movieReadModelRepository;
        private readonly IRepository<TicketReadModel, Guid> ticketReadModelRepository;

        public ReservedSeatsNotificationHandler(
            IReadRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository,
            IReadRepository<AuditoriumReadModel, Guid> auditoriumReadModelRepository,
            IReadRepository<MovieReadModel, Guid> movieReadModelRepository,
            IRepository<TicketReadModel, Guid> ticketReadModelRepository)
        {
            this.showtimeReadModelRepository = Guard.Against.Null(showtimeReadModelRepository, nameof(showtimeReadModelRepository));
            this.auditoriumReadModelRepository = Guard.Against.Null(auditoriumReadModelRepository, nameof(auditoriumReadModelRepository));
            this.movieReadModelRepository = Guard.Against.Null(movieReadModelRepository, nameof(movieReadModelRepository));
            this.ticketReadModelRepository = Guard.Against.Null(ticketReadModelRepository, nameof(ticketReadModelRepository));
        }

        public async Task Handle(ReservedSeatsNotification notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            var @event = notification.Event;

            var existingShowtime = await this.showtimeReadModelRepository.GetByIdAsync(new ShowtimeId(@event.AggregateId), cancellationToken);
            if (existingShowtime is null)
            {
                throw new NotFoundException(nameof(Showtime), @event.AggregateId.ToString());
            }

            var existingMovie = await this.movieReadModelRepository.GetByIdAsync(new MovieId(existingShowtime.MovieId), cancellationToken);
            if (existingMovie is null)
            {
                throw new NotFoundException(nameof(Movie), existingShowtime.MovieId.ToString()!);
            }

            var existingAuditorium = await this.auditoriumReadModelRepository.GetByIdAsync(new AuditoriumId(existingShowtime.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                throw new NotFoundException(nameof(Auditorium), existingShowtime.AuditoriumId.ToString()!);
            }

            var seats = existingAuditorium.Seats.Where(seat => @event.SeatIds.Contains(seat.Id))
                .Select(x => new SeatReadModel(x.Id, x.Row, x.SeatNumber)).ToList();

            var ticketReadModel = new TicketReadModel(
                @event.TicketId,
                @event.UserId,
                @event.AggregateId,
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