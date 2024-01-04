namespace JordiAragon.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public class EndOfPeriod : BaseValueObject
    {
        private EndOfPeriod(DateTimeOffset value)
        {
            Guard.Against.Default(value, nameof(value));

            this.Value = value;
        }

        public DateTimeOffset Value { get; init; }

        public static implicit operator DateTimeOffset(EndOfPeriod endOfPeriod)
        {
            Guard.Against.Null(endOfPeriod, nameof(endOfPeriod));

            return endOfPeriod.Value;
        }

        public static explicit operator EndOfPeriod(DateTimeOffset value)
        {
            return new EndOfPeriod(value);
        }

        public static EndOfPeriod Create(DateTimeOffset value)
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