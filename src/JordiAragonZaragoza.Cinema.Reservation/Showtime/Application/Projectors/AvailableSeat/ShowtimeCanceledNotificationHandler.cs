﻿namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.AvailableSeat
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class ShowtimeCanceledNotificationHandler : IEventNotificationHandler<ShowtimeCanceledNotification>
    {
        private readonly IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository;
        private readonly ISpecificationReadRepository<AvailableSeatReadModel, Guid> availableReadModelSpecificationRepository;

        public ShowtimeCanceledNotificationHandler(
            ISpecificationReadRepository<AvailableSeatReadModel, Guid> availableReadModelSpecificationRepository,
            IRangeableRepository<AvailableSeatReadModel, Guid> availableReadModelRepository)
        {
            this.availableReadModelRepository = Guard.Against.Null(availableReadModelRepository, nameof(availableReadModelRepository));
            this.availableReadModelSpecificationRepository = Guard.Against.Null(availableReadModelSpecificationRepository, nameof(availableReadModelSpecificationRepository));
        }

        public async Task Handle(ShowtimeCanceledNotification notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            var @event = notification.Event;

            var availableSeats = await this.availableReadModelSpecificationRepository.ListAsync(new GetAvailableSeatsByShowtimeIdSpec(@event.AggregateId), cancellationToken);

            await this.availableReadModelRepository.DeleteRangeAsync(availableSeats, cancellationToken);
        }
    }
}