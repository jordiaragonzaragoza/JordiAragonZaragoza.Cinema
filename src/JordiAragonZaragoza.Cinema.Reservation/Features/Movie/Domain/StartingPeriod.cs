namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

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

        public static DateTimeOffset FromStartingPeriod(StartingPeriod startingPeriod)
        {
            return startingPeriod;
        }

        public static StartingPeriod ToStartingPeriod(DateTimeOffset value)
        {
            return (StartingPeriod)value;
        }

        public static StartingPeriod Create(DateTimeOffset value)
            => new(value);

        public override string ToString()
        {
            return this.Value.ToString(CultureInfo.InvariantCulture);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}