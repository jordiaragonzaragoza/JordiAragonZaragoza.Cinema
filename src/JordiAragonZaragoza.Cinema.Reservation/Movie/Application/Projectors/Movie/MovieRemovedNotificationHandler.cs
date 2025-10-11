namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Projectors.Movie
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class MovieRemovedNotificationHandler : BaseEventHandler<MovieRemovedEvent>
    {
        private readonly IRepository<MovieReadModel, Guid> movieReadModelRepository;

        public MovieRemovedNotificationHandler(
            IRepository<MovieReadModel, Guid> movieReadModelRepository)
        {
            this.movieReadModelRepository = movieReadModelRepository ?? throw new ArgumentNullException(nameof(movieReadModelRepository));
        }

        public override async Task HandleAsync(MovieRemovedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var readModel = await this.movieReadModelRepository.GetByIdAsync(@event.AggregateId, cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(MovieReadModel), @event.AggregateId.ToString());
            }

            await this.movieReadModelRepository.DeleteAsync(readModel, cancellationToken);
        }
    }
}