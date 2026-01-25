namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Reservation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class ShowtimeCanceledEventProjector : BaseEventHandler<ShowtimeCanceledEvent>
    {
        private readonly IRangeableRepository<ReservationReadModel, Guid> reservationReadModelRepository;
        private readonly ISpecificationReadRepository<ReservationReadModel, Guid> specificationRepository;

        public ShowtimeCanceledEventProjector(
            IRangeableRepository<ReservationReadModel, Guid> reservationReadModelRepository,
            ISpecificationReadRepository<ReservationReadModel, Guid> specificationRepository)
        {
            this.reservationReadModelRepository = reservationReadModelRepository ?? throw new ArgumentNullException(nameof(reservationReadModelRepository));
            this.specificationRepository = specificationRepository ?? throw new ArgumentNullException(nameof(specificationRepository));
        }

        public override async Task HandleAsync(ShowtimeCanceledEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var specification = new GetReservationsByShowtimeIdSpec(@event.AggregateId);
            var existingReservations = await this.specificationRepository.ListAsync(specification, cancellationToken);
            if (existingReservations.Count > 0)
            {
                await this.reservationReadModelRepository.DeleteRangeAsync(existingReservations, cancellationToken);
            }
        }
    }
}