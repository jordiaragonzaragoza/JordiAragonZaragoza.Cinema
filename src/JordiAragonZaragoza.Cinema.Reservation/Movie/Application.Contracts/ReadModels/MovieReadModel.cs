namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.ReadModels;

    public sealed class MovieReadModel : BaseReadModel
    {
        public MovieReadModel(
            Guid id,
            string title,
            TimeSpan runtime)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
            this.Title = Guard.Against.Default(title, nameof(title));
            this.Runtime = Guard.Against.Default(runtime, nameof(runtime));
        }

        // Required by EF.
        private MovieReadModel()
        {
        }

        public string Title { get; private set; } = default!;

        public TimeSpan Runtime { get; private set; }
    }
}