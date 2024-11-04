namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class EndOfPeriod : BaseValueObject
    {
        // TODO: Restrict via ArchTest to only accessible  on current aggregate.
        internal EndOfPeriod(DateTimeOffset value)
            => this.Value = value;

        public DateTimeOffset Value { get; init; }

        public static implicit operator DateTimeOffset(EndOfPeriod endOfPeriod)
        {
            ArgumentNullException.ThrowIfNull(endOfPeriod, nameof(endOfPeriod));

            return endOfPeriod.Value;
        }

        public static DateTimeOffset FromEndOfPeriod(EndOfPeriod endOfPeriod)
            => endOfPeriod;

        public static EndOfPeriod Create(DateTimeOffset value)
        {
            Guard.Against.Default(value, nameof(value));

            return new EndOfPeriod(value);
        }

        public override string ToString()
            => this.Value.ToString(CultureInfo.InvariantCulture);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}