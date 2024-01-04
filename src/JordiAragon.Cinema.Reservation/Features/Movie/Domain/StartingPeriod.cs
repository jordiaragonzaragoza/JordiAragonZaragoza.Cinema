namespace JordiAragon.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Guards;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public class StartingPeriod : BaseValueObject
    {
        private StartingPeriod(DateTime value)
        {
            Guard.Against.Default(value, nameof(value));
            Guard.Against.NotUtc(value, nameof(value));

            this.Value = value;
        }

        public DateTime Value { get; init; }

        public static implicit operator DateTime(StartingPeriod startingPeriod)
        {
            Guard.Against.Null(startingPeriod, nameof(startingPeriod));

            return startingPeriod.Value;
        }

        public static explicit operator StartingPeriod(DateTime value)
        {
            return new StartingPeriod(value);
        }

        public static StartingPeriod Create(DateTime value)
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