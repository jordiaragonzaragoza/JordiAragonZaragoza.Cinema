namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Projectors.Movie
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class MovieAddedNotificationHandler : BaseEventHandler<MovieAddedEvent>
    {
        private readonly IRepository<MovieReadModel, Guid> movieReadModelRepository;

        public MovieAddedNotificationHandler(
            IRepository<MovieReadModel, Guid> movieReadModelRepository)
        {
            this.movieReadModelRepository = movieReadModelRepository ?? throw new ArgumentNullException(nameof(movieReadModelRepository));
        }

        public override async Task HandleAsync(MovieAddedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var movieReadModel = new MovieReadModel(
                @event.AggregateId,
                @event.Title,
                @event.Runtime);

            await this.movieReadModelRepository.AddAsync(movieReadModel, cancellationToken);
        }
    }
}