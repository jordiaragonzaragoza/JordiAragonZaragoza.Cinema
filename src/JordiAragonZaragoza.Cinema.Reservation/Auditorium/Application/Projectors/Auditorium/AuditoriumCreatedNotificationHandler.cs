namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Projectors.Auditorium
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Notifications;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class AuditoriumCreatedNotificationHandler : IEventNotificationHandler<AuditoriumCreatedNotification>
    {
        private readonly IRepository<AuditoriumReadModel, Guid> auditoriumReadModelRepository;

        public AuditoriumCreatedNotificationHandler(
            IRepository<AuditoriumReadModel, Guid> auditoriumReadModelRepository)
        {
            this.auditoriumReadModelRepository = Guard.Against.Null(auditoriumReadModelRepository, nameof(auditoriumReadModelRepository));
        }

        public async Task Handle(AuditoriumCreatedNotification notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            var @event = notification.Event;

            var seats = @event.SeatIds
                .Select((seatId, index) => new SeatReadModel(
                    seatId,
                    @event.SeatRows[index],
                    @event.SeatNumbers[index]))
                .ToList();

            var auditoriumReadModel = new AuditoriumReadModel(
                @event.AggregateId,
                @event.Name,
                seats);

            await this.auditoriumReadModelRepository.AddAsync(auditoriumReadModel, cancellationToken);
        }
    }
}