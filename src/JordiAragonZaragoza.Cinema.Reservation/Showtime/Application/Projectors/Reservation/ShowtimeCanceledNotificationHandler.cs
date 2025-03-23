namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Reservation
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
        private readonly IRangeableRepository<ReservationReadModel, Guid> reservationReadModelRepository;
        private readonly ISpecificationReadRepository<ReservationReadModel, Guid> specificationRepository;

        public ShowtimeCanceledNotificationHandler(
            IRangeableRepository<ReservationReadModel, Guid> reservationReadModelRepository,
            ISpecificationReadRepository<ReservationReadModel, Guid> specificationRepository)
        {
            this.reservationReadModelRepository = Guard.Against.Null(reservationReadModelRepository, nameof(reservationReadModelRepository));
            this.specificationRepository = Guard.Against.Null(specificationRepository, nameof(specificationRepository));
        }

        public async Task Handle(ShowtimeCanceledNotification notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            var @event = notification.Event;

            var specification = new GetReservationsByShowtimeIdSpec(@event.AggregateId);
            var existingReservations = await this.specificationRepository.ListAsync(specification, cancellationToken);
            if (existingReservations.Count > 0)
            {
                await this.reservationReadModelRepository.DeleteRangeAsync(existingReservations, cancellationToken);
            }
        }
    }
}