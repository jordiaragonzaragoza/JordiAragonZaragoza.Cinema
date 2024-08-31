namespace JordiAragon.Cinema.Reservation.Movie.Application.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ShowtimeScheduledEventHandler : IEventHandler<ShowtimeScheduledEvent>
    {
        private readonly IRepository<Movie, MovieId> movieRepository;

        public ShowtimeScheduledEventHandler(
            IRepository<Movie, MovieId> movieRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
        }

        public async Task Handle(ShowtimeScheduledEvent notification, CancellationToken cancellationToken)
        {
            Guard.Against.Null(notification, nameof(notification));

            var existingMovie = await this.movieRepository.GetByIdAsync(MovieId.Create(notification.MovieId), cancellationToken)
                                ?? throw new NotFoundException(nameof(Movie), notification.MovieId.ToString());

            existingMovie.AddActiveShowtime(ShowtimeId.Create(notification.AggregateId));

            await this.movieRepository.UpdateAsync(existingMovie, cancellationToken);
        }
    }
}