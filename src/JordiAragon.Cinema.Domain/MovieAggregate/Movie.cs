namespace JordiAragon.Cinema.Domain.MovieAggregate
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.MovieAggregate.Events;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;

    public class Movie : BaseAggregateRoot<MovieId, Guid>, IAggregateRoot
    {
        private readonly List<ShowtimeId> showtimes = new();

        private Movie(
            MovieId id,
            string title,
            string imdbId,
            DateTime releaseDateOnUtc,
            string stars)
            : base(id)
        {
            this.Title = Guard.Against.NullOrEmpty(title, nameof(title));
            this.ImdbId = Guard.Against.NullOrEmpty(imdbId, nameof(imdbId));
            this.ReleaseDateOnUtc = releaseDateOnUtc;
            this.Stars = Guard.Against.NullOrEmpty(stars, nameof(stars));

            this.RegisterDomainEvent(new MovieCreatedEvent(id, this.Title, this.ImdbId, this.ReleaseDateOnUtc, this.Stars));
        }

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
            return new Movie(id, title, imdbId, releaseDateOnUtc, stars);
        }
    }
}