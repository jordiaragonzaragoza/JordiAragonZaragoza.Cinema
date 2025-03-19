namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.AvailableSeat
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ExpiredReservedSeatsNotificationHandler : IEventNotificationHandler<ExpiredReservedSeatsNotification>
    {
        private readonly IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository;
        private readonly IReadRepository<ShowtimeReadModel, Guid> showtimeReadRepository;
        private readonly IReadRepository<AuditoriumReadModel, Guid> auditoriumReadRepository;
        private readonly IIdGenerator guidGenerator;

        public ExpiredReservedSeatsNotificationHandler(
            IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository,
            ISpecificationReadRepository<AvailableSeatReadModel, Guid> specificationReadRepository,
            IReadRepository<ShowtimeReadModel, Guid> showtimeReadRepository,
            IReadRepository<AuditoriumReadModel, Guid> auditoriumReadRepository,
            IIdGenerator guidGenerator)
        {
            this.availableReadModelRepository = Guard.Against.Null(availableReadModelRepository, nameof(availableReadModelRepository));
            this.showtimeReadRepository = Guard.Against.Null(showtimeReadRepository, nameof(showtimeReadRepository));
            this.auditoriumReadRepository = Guard.Against.Null(auditoriumReadRepository, nameof(auditoriumReadRepository));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
        }

        public async Task Handle(ExpiredReservedSeatsNotification notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            var @event = notification.Event;

            var existingShowtime = await this.showtimeReadRepository.GetByIdAsync(new ShowtimeId(@event.AggregateId), cancellationToken);
            if (existingShowtime is null)
            {
                throw new NotFoundException(nameof(Showtime), @event.AggregateId.ToString());
            }

            var existingAuditorium = await this.auditoriumReadRepository.GetByIdAsync(new AuditoriumId(existingShowtime.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                throw new NotFoundException(nameof(Auditorium), existingShowtime.AuditoriumId.ToString()!);
            }

            var seatIds = @event.SeatIds.ToHashSet();

            var newAvailableSeats = existingAuditorium.Seats
                .Where(seat => seatIds.Contains(seat.Id))
                .Select(seat => new AvailableSeatReadModel(
                    this.guidGenerator.Create(),
                    seat.Id,
                    seat.Row,
                    seat.SeatNumber,
                    @event.AggregateId,
                    existingAuditorium.Id,
                    existingAuditorium.Name));

            await this.availableReadModelRepository.AddRangeAsync(newAvailableSeats, cancellationToken);
        }
    }
}