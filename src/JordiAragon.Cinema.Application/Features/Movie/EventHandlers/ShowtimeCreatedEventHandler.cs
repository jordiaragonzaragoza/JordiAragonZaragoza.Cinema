﻿namespace JordiAragon.Cinema.Application.Features.Movie.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate.Specifications;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate.Events;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class ShowtimeCreatedEventHandler : IDomainEventHandler<ShowtimeCreatedEvent>
    {
        private readonly IRepository<Movie> movieRepository;

        public ShowtimeCreatedEventHandler(
            IRepository<Movie> movieRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
        }

        public async Task Handle(ShowtimeCreatedEvent @event, CancellationToken cancellationToken)
        {
            var existingMovie = await this.movieRepository.FirstOrDefaultAsync(new MovieByIdSpec(@event.MovieId), cancellationToken);
            if (existingMovie is null)
            {
                return; // TODO: Complete.
            }

            existingMovie.AddShowtime(@event.ShowtimeId);

            await this.movieRepository.UpdateAsync(existingMovie, cancellationToken);
        }
    }
}