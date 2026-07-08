namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Projectors.Cinema
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class CinemaRemovedEventProjector : BaseEventHandler<CinemaRemovedEvent>
    {
        private readonly IRepository<CinemaReadModel, Guid> cinemaReadModelRepository;

        public CinemaRemovedEventProjector(
            IRepository<CinemaReadModel, Guid> cinemaReadModelRepository)
        {
            this.cinemaReadModelRepository = cinemaReadModelRepository ?? throw new ArgumentNullException(nameof(cinemaReadModelRepository));
        }

        public override async Task HandleAsync(CinemaRemovedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var readModel = await this.cinemaReadModelRepository.GetByIdAsync(@event.AggregateId, cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(CinemaReadModel), @event.AggregateId.ToString());
            }

            await this.cinemaReadModelRepository.DeleteAsync(readModel, cancellationToken);
        }
    }
}