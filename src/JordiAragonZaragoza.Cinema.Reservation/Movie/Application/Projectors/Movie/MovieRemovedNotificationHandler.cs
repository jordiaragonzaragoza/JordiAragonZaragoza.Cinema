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

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class MovieRemovedNotificationHandler : IEventNotificationHandler<MovieRemovedNotification>
    {
        private readonly IRepository<MovieReadModel, Guid> movieReadModelRepository;

        public MovieRemovedNotificationHandler(
            IRepository<MovieReadModel, Guid> movieReadModelRepository)
        {
            this.movieReadModelRepository = Guard.Against.Null(movieReadModelRepository, nameof(movieReadModelRepository));
        }

        public async Task Handle(MovieRemovedNotification notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification, nameof(notification));

            var @event = notification.Event;

            var readModel = await this.movieReadModelRepository.GetByIdAsync(@event.AggregateId, cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(MovieReadModel), @event.AggregateId.ToString());
            }

            await this.movieReadModelRepository.DeleteAsync(readModel, cancellationToken);
        }
    }
}