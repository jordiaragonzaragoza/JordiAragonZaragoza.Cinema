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

    public sealed class ShowtimeScheduledEventHandler : BaseEventHandler<ShowtimeScheduledEvent>
    {
        private readonly IRepository<Movie, MovieId> movieRepository;

        public ShowtimeScheduledEventHandler(
            IRepository<Movie, MovieId> movieRepository)
        {
            this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
        }

        public override async Task HandleAsync(ShowtimeScheduledEvent notification, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(notification);

            var existingMovie = await this.movieRepository.GetByIdAsync(new MovieId(notification.MovieId), cancellationToken)
                                ?? throw new NotFoundException(nameof(Movie), notification.MovieId.ToString());

            existingMovie.AddActiveShowtime(new ShowtimeId(notification.AggregateId));

            await this.movieRepository.UpdateAsync(existingMovie, cancellationToken);
        }
    }
}