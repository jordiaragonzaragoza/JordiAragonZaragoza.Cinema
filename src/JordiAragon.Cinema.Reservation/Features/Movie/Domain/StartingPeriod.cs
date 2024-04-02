namespace JordiAragon.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class StartingPeriod : BaseValueObject
    {
        private StartingPeriod(DateTimeOffset value)
        {
            Guard.Against.Default(value, nameof(value));

            this.Value = value;
        }

        public DateTimeOffset Value { get; init; }

        public static implicit operator DateTimeOffset(StartingPeriod startingPeriod)
        {
            Guard.Against.Null(startingPeriod, nameof(startingPeriod));

            return startingPeriod.Value;
        }

        public static explicit operator StartingPeriod(DateTimeOffset value)
        {
            return new StartingPeriod(value);
        }

        public static StartingPeriod Create(DateTimeOffset value)
            => new(value);

        public override string ToString()
        {
            return this.Value.ToString();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}