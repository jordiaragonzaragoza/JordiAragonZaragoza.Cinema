namespace JordiAragon.Cinemas.Ticketing.Movie.Domain
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class MovieId : BaseAggregateRootId<Guid>
    {
        private MovieId(Guid value)
            : base(value)
        {
        }

        public static MovieId Create(Guid id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            return new MovieId(id);
        }
    }
}