namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class SessionDate : BaseValueObject
    {
        internal SessionDate(DateTimeOffset value)
            => this.Value = value;

        public DateTimeOffset Value { get; init; }

        public static implicit operator DateTimeOffset(SessionDate sessionDate)
        {
            ArgumentNullException.ThrowIfNull(sessionDate, nameof(sessionDate));

            return sessionDate.Value;
        }

        public static DateTimeOffset FromSessionDate(SessionDate sessionDate)
        {
            return sessionDate;
        }

        public static SessionDate Create(DateTimeOffset value)
        {
            // Here we can add more validation rules.
            Guard.Against.Default(value, nameof(value));
            return new SessionDate(value);
        }

        public override string ToString()
            => this.Value.ToString(CultureInfo.InvariantCulture);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}