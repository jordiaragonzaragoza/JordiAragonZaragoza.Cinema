namespace JordiAragon.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Guards;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public class EndOfPeriod : BaseValueObject
    {
        private EndOfPeriod(DateTime value)
        {
            Guard.Against.Default(value, nameof(value));
            Guard.Against.NotUtc(value, nameof(value));

            this.Value = value;
        }

        public DateTime Value { get; init; }

        public static implicit operator DateTime(EndOfPeriod endOfPeriod)
        {
            Guard.Against.Null(endOfPeriod, nameof(endOfPeriod));

            return endOfPeriod.Value;
        }

        public static explicit operator EndOfPeriod(DateTime value)
        {
            return new EndOfPeriod(value);
        }

        public static EndOfPeriod Create(DateTime value)
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