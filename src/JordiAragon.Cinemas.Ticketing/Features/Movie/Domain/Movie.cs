namespace JordiAragon.Cinemas.Ticketing.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinemas.Ticketing.Movie.Domain.Events;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
    using JordiAragon.SharedKernel.Domain.Exceptions;

    public class Movie : BaseAggregateRoot<MovieId, Guid>
    {
        private readonly List<ShowtimeId> showtimes = new();

        // Required by EF.
        private Movie()
        {
        }

        public string Title { get; private set; }

        public string ImdbId { get; private set; }

        public string Stars { get; private set; }

        public DateTime ReleaseDateOnUtc { get; private set; }

        public IEnumerable<ShowtimeId> Showtimes => this.showtimes.AsReadOnly();

        public static Movie Create(MovieId id, string title, string imdbId, DateTime releaseDateOnUtc, string stars)
        {
            var movie = new Movie();

            movie.Apply(new MovieCreatedEvent(id, title, imdbId, releaseDateOnUtc, stars));

            return movie;
        }

        public void AddShowtime(ShowtimeId showtimeId)
            => this.Apply(new ShowtimeAddedEvent(this.Id, showtimeId));

        public void RemoveShowtime(ShowtimeId showtimeId)
            => this.Apply(new ShowtimeRemovedEvent(this.Id, showtimeId));

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case MovieCreatedEvent @event:
                    this.Applier(@event);
                    break;

                case ShowtimeAddedEvent @event:
                    this.showtimes.Add(ShowtimeId.Create(@event.ShowtimeId));
                    break;

                case ShowtimeRemovedEvent @event:
                    this.showtimes.Remove(ShowtimeId.Create(@event.ShowtimeId));
                    break;
            }
        }

        protected override void EnsureValidState()
        {
            try
            {
                Guard.Against.Null(this.Id, nameof(this.Id));
                Guard.Against.NullOrWhiteSpace(this.Title, nameof(this.Title));
                Guard.Against.NullOrWhiteSpace(this.ImdbId, nameof(this.ImdbId));
                Guard.Against.Default(this.ReleaseDateOnUtc, nameof(this.ReleaseDateOnUtc));
                Guard.Against.NullOrWhiteSpace(this.Stars, nameof(this.Stars));
            }
            catch (Exception exception)
            {
                throw new InvalidAggregateStateException(this, exception.Message);
            }
        }

        private void Applier(MovieCreatedEvent @event)
        {
            this.Id = MovieId.Create(@event.AggregateId);
            this.Title = @event.Title;
            this.ImdbId = @event.ImdbId;
            this.ReleaseDateOnUtc = @event.ReleaseDateOnUtc;
            this.Stars = @event.Stars;
        }
    }
}