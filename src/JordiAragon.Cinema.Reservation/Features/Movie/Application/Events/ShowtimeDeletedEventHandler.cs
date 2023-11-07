namespace JordiAragon.Cinema.Reservation.Movie.Application.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Events;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain.Specifications;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;
    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public class ShowtimeDeletedEventHandler : INotificationHandler<ShowtimeDeletedEvent>
    {
        private readonly IRepository<Movie> movieRepository;

        public ShowtimeDeletedEventHandler(
            IRepository<Movie> movieRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
        }

        public async Task Handle(ShowtimeDeletedEvent @event, CancellationToken cancellationToken)
        {
            var existingMovie = await this.movieRepository.FirstOrDefaultAsync(new MovieByIdSpec(MovieId.Create(@event.MovieId)), cancellationToken)
                                    ?? throw new NotFoundException(nameof(Movie), @event.MovieId.ToString());

            existingMovie.RemoveShowtime(ShowtimeId.Create(@event.ShowtimeId));

            await this.movieRepository.UpdateAsync(existingMovie, cancellationToken);
        }
    }
}