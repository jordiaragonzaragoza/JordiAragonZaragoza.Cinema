namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.AvailableSeat
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class ShowtimeEndedNotificationHandler : BaseEventHandler<ShowtimeEndedEvent>
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

        public override async Task HandleAsync(ShowtimeEndedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var availableSeats = await this.availableReadModelSpecificationRepository.ListAsync(new GetAvailableSeatsByShowtimeIdSpec(@event.AggregateId), cancellationToken);

            await this.availableReadModelRepository.DeleteRangeAsync(availableSeats, cancellationToken);
        }
    }
}