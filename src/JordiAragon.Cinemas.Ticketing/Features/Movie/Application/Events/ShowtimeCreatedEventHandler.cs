namespace JordiAragon.Cinemas.Ticketing.Movie.Application.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinemas.Ticketing.Movie.Domain;
    using JordiAragon.Cinemas.Ticketing.Movie.Domain.Specifications;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain.Events;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;
    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public class ShowtimeCreatedEventHandler : INotificationHandler<ShowtimeCreatedEvent>
    {
        private readonly IRepository<Movie> movieRepository;

        public ShowtimeCreatedEventHandler(
            IRepository<Movie> movieRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
        }

        public async Task Handle(ShowtimeCreatedEvent @event, CancellationToken cancellationToken)
        {
            var existingMovie = await this.movieRepository.FirstOrDefaultAsync(new MovieByIdSpec(MovieId.Create(@event.MovieId)), cancellationToken)
                                ?? throw new NotFoundException(nameof(Movie), @event.MovieId.ToString());

            existingMovie.AddShowtime(ShowtimeId.Create(@event.ShowtimeId));

            await this.movieRepository.UpdateAsync(existingMovie, cancellationToken);
        }
    }
}