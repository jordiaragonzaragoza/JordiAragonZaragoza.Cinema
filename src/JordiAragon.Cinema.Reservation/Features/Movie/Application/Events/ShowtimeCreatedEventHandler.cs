namespace JordiAragon.Cinema.Reservation.Movie.Application.Events
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using MediatR;
    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public class ShowtimeCreatedEventHandler : INotificationHandler<ShowtimeCreatedEvent>
    {
        private readonly IRepository<Movie, MovieId> movieRepository;

        public ShowtimeCreatedEventHandler(
            IRepository<Movie, MovieId> movieRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
        }

        public async Task Handle(ShowtimeCreatedEvent @event, CancellationToken cancellationToken)
        {
            var existingMovie = await this.movieRepository.GetByIdAsync(MovieId.Create(@event.MovieId), cancellationToken)
                                ?? throw new NotFoundException(nameof(Movie), @event.MovieId.ToString());

            existingMovie.AddShowtime(ShowtimeId.Create(@event.ShowtimeId));

            await this.movieRepository.UpdateAsync(existingMovie, cancellationToken);
        }
    }
}