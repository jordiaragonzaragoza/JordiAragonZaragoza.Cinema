namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class ShowtimeReadModel : IReadModel
    {
        public ShowtimeReadModel(
            Guid id,
            DateTimeOffset sessionDateOnUtc,
            Guid movieId,
            string movieTitle,
            TimeSpan movieRuntime,
            Guid auditoriumId,
            string auditoriumName)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
            this.SessionDateOnUtc = Guard.Against.Default(sessionDateOnUtc, nameof(sessionDateOnUtc));
            this.MovieId = Guard.Against.Default(movieId, nameof(movieId));
            this.MovieTitle = Guard.Against.NullOrWhiteSpace(movieTitle, nameof(movieTitle));
            this.MovieRuntime = Guard.Against.Default(movieRuntime, nameof(movieRuntime));
            this.AuditoriumId = Guard.Against.Default(auditoriumId, nameof(auditoriumId));
            this.AuditoriumName = Guard.Against.NullOrWhiteSpace(auditoriumName, nameof(auditoriumName));
        }

        // Required by EF.
        private ShowtimeReadModel()
        {
        }

        public Guid Id { get; private set; }

        public DateTimeOffset SessionDateOnUtc { get; private set; }

        public Guid MovieId { get; private set; }

        public string MovieTitle { get; private set; } = string.Empty;

        public TimeSpan MovieRuntime { get; private set; }

        public Guid AuditoriumId { get; private set; }

        public string AuditoriumName { get; private set; } = string.Empty;
    }
}