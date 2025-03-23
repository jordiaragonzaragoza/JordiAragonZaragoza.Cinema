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

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ExpiredReservedSeatsNotificationHandler : IEventNotificationHandler<ExpiredReservedSeatsNotification>
    {
        private readonly IRepository<ReservationReadModel, Guid> reservationReadModelRepository;

        public ExpiredReservedSeatsNotificationHandler(
            IRepository<ReservationReadModel, Guid> reservationReadModelRepository)
        {
            this.reservationReadModelRepository = Guard.Against.Null(reservationReadModelRepository, nameof(reservationReadModelRepository));
        }

        public async Task Handle(ExpiredReservedSeatsNotification notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            var @event = notification.Event;

            var existingReservation = await this.reservationReadModelRepository.GetByIdAsync(@event.ReservationId, cancellationToken);
            if (existingReservation is null)
            {
                throw new NotFoundException(nameof(ReservationReadModel), @event.ReservationId.ToString());
            }

            await this.reservationReadModelRepository.DeleteAsync(existingReservation, cancellationToken);
        }
    }
}