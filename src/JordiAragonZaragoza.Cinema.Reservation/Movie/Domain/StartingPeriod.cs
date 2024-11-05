namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class StartingPeriod : BaseValueObject
    {
        internal StartingPeriod(DateTimeOffset value)
            => this.Value = value;

        public DateTimeOffset Value { get; init; }

        public static implicit operator DateTimeOffset(StartingPeriod startingPeriod)
        {
            ArgumentNullException.ThrowIfNull(startingPeriod, nameof(startingPeriod));

            return startingPeriod.Value;
        }

        public static DateTimeOffset FromStartingPeriod(StartingPeriod startingPeriod)
        {
            return startingPeriod;
        }

        public static StartingPeriod Create(DateTimeOffset value)
        {
            CheckRule(new MinimumStartingPeriodRule(value));

            return new StartingPeriod(value);
        }

        public override string ToString()
            => this.Value.ToString(CultureInfo.InvariantCulture);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}