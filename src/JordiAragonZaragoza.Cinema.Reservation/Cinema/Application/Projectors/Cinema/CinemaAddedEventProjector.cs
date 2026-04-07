namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Projectors.Cinema
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class CinemaAddedEventProjector : BaseEventHandler<CinemaCreatedEvent>
    {
        private readonly IRepository<CinemaReadModel, Guid> cinemaReadModelRepository;

        public CinemaAddedEventProjector(
            IRepository<CinemaReadModel, Guid> cinemaReadModelRepository)
        {
            this.cinemaReadModelRepository = cinemaReadModelRepository ?? throw new ArgumentNullException(nameof(cinemaReadModelRepository));
        }

        public override async Task HandleAsync(CinemaCreatedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var cinemaReadModel = new CinemaReadModel(
                @event.AggregateId);

            await this.cinemaReadModelRepository.AddAsync(cinemaReadModel, cancellationToken);
        }
    }
}