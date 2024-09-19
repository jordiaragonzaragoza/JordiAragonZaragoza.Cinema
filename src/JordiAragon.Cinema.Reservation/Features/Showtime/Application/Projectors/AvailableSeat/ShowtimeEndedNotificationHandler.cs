namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projectors.AvailableSeat
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public sealed class ShowtimeEndedNotificationHandler : IEventNotificationHandler<ShowtimeEndedNotification>
    {
        private readonly IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository;
        private readonly ISpecificationReadRepository<AvailableSeatReadModel, Guid> availableReadModelSpecificationRepository;

        public ShowtimeEndedNotificationHandler(
            ISpecificationReadRepository<AvailableSeatReadModel, Guid> availableReadModelSpecificationRepository,
            IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository)
        {
            this.availableReadModelRepository = Guard.Against.Null(availableReadModelRepository, nameof(availableReadModelRepository));
            this.availableReadModelSpecificationRepository = Guard.Against.Null(availableReadModelSpecificationRepository, nameof(availableReadModelSpecificationRepository));
        }

        public async Task Handle(ShowtimeEndedNotification notification, CancellationToken cancellationToken)
        {
            Guard.Against.Null(notification, nameof(notification));

            var @event = notification.Event;

            var availableSeats = await this.availableReadModelSpecificationRepository.ListAsync(new GetAvailableSeatsByShowtimeIdSpec(@event.AggregateId), cancellationToken);

            await this.availableReadModelRepository.DeleteRangeAsync(availableSeats, cancellationToken);
        }
    }
}