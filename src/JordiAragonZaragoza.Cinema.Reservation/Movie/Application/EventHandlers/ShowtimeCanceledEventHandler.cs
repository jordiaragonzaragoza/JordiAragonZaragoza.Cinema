namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.EventHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ShowtimeCanceledEventHandler : BaseEventHandler<ShowtimeCanceledEvent>
    {
        private readonly IRepository<Movie, MovieId> movieRepository;

        public ShowtimeCanceledEventHandler(
            IRepository<Movie, MovieId> movieRepository)
        {
            this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
        }

        public override async Task HandleAsync(ShowtimeCanceledEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event, nameof(@event));

            var existingMovie = await this.movieRepository.GetByIdAsync(new MovieId(@event.MovieId), cancellationToken)
                                    ?? throw new NotFoundException(nameof(Movie), @event.MovieId.ToString());

            existingMovie.RemoveActiveShowtime(new ShowtimeId(@event.AggregateId));

            await this.movieRepository.UpdateAsync(existingMovie, cancellationToken);
        }
    }
}