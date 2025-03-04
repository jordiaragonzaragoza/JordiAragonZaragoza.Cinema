namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Projectors.Movie
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Notifications;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class MovieAddedNotificationHandler : IEventNotificationHandler<MovieAddedNotification>
    {
        private readonly IRepository<MovieReadModel, Guid> movieReadModelRepository;

        public MovieAddedNotificationHandler(
            IRepository<MovieReadModel, Guid> movieReadModelRepository)
        {
            this.movieReadModelRepository = Guard.Against.Null(movieReadModelRepository, nameof(movieReadModelRepository));
        }

        public async Task Handle(MovieAddedNotification notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            var @event = notification.Event;

            var movieReadModel = new MovieReadModel(
                @event.AggregateId,
                @event.Title,
                @event.Runtime);

            await this.movieReadModelRepository.AddAsync(movieReadModel, cancellationToken);
        }
    }
}