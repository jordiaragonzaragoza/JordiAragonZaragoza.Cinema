namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Reservation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class PurchasedReservationEventProjector : BaseEventHandler<PurchasedReservationEvent>
    {
        private readonly IRepository<ReservationReadModel, Guid> reservationReadModelRepository;

        public PurchasedReservationEventProjector(
            IRepository<ReservationReadModel, Guid> reservationReadModelRepository)
        {
            this.reservationReadModelRepository = reservationReadModelRepository ?? throw new ArgumentNullException(nameof(reservationReadModelRepository));
        }

        public override async Task HandleAsync(PurchasedReservationEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var existingReservation = await this.reservationReadModelRepository.GetByIdAsync(@event.ReservationId, cancellationToken);
            if (existingReservation is null)
            {
                throw new NotFoundException(nameof(ReservationReadModel), @event.ReservationId.ToString());
            }

            existingReservation.IsPurchased = true;

            await this.reservationReadModelRepository.UpdateAsync(existingReservation, cancellationToken);
        }
    }
}