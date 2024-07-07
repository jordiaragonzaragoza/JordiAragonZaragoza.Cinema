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

        public async Task Handle(ShowtimeScheduledEvent @event, CancellationToken cancellationToken)
        {
            var existingMovie = await this.movieRepository.GetByIdAsync(MovieId.Create(@event.MovieId), cancellationToken)
                                ?? throw new NotFoundException(nameof(Movie), @event.MovieId.ToString());

            existingMovie.AddShowtime(ShowtimeId.Create(@event.AggregateId));

            await this.movieRepository.UpdateAsync(existingMovie, cancellationToken);
        }
    }
}