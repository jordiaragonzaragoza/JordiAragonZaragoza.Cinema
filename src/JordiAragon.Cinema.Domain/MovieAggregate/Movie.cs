namespace JordiAragon.Cinema.Domain.MovieAggregate
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;

    public class Movie : BaseAuditableEntity<MovieId>, IAggregateRoot
    {
        private readonly List<Showtime> showtimes = new();

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
        }

        public string Title { get; private set; }

        public string ImdbId { get; private set; }

        public string Stars { get; private set; }

        public DateTime ReleaseDateOnUtc { get; private set; }

        public IEnumerable<Showtime> Showtimes => this.showtimes.AsReadOnly();

        public static Movie Create(MovieId id, string title, string imdbId, DateTime releaseDateOnUtc, string stars)
        {
            return new Movie(id, title, imdbId, releaseDateOnUtc, stars);
        }
    }
}