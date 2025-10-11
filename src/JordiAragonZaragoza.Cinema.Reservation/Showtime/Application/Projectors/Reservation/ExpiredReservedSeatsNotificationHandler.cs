namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Reservation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ExpiredReservedSeatsNotificationHandler : BaseEventHandler<ExpiredReservedSeatsEvent>
    {
        private readonly IRepository<ReservationReadModel, Guid> reservationReadModelRepository;

        public ExpiredReservedSeatsNotificationHandler(
            IRepository<ReservationReadModel, Guid> reservationReadModelRepository)
        {
            this.reservationReadModelRepository = Guard.Against.Null(reservationReadModelRepository, nameof(reservationReadModelRepository));
        }

        public override async Task HandleAsync(ExpiredReservedSeatsEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var existingReservation = await this.reservationReadModelRepository.GetByIdAsync(@event.ReservationId, cancellationToken);
            if (existingReservation is null)
            {
                throw new NotFoundException(nameof(ReservationReadModel), @event.ReservationId.ToString());
            }

            await this.reservationReadModelRepository.DeleteAsync(existingReservation, cancellationToken);
        }
    }
}