namespace JordiAragon.Cinema.Domain.MovieAggregate
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class MovieId : BaseValueObject
    {
        private MovieId(Guid value)
        {
            this.Value = value;
        }

        public Guid Value { get; private set; }

        public static MovieId Create(Guid id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            return new MovieId(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}