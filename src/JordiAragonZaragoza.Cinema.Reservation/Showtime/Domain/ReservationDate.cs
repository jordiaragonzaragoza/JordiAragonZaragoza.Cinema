namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class ReservationDate : BaseValueObject
    {
        internal ReservationDate(DateTimeOffset value)
            => this.Value = value;

        public DateTimeOffset Value { get; init; }

        public static implicit operator DateTimeOffset(ReservationDate reservationDate)
        {
            ArgumentNullException.ThrowIfNull(reservationDate, nameof(reservationDate));

            return reservationDate.Value;
        }

        public static DateTimeOffset FromReservationDate(ReservationDate reservationDate)
        {
            return reservationDate;
        }

        public static ReservationDate Create(DateTimeOffset value)
        {
            // Here we can add more validation rules.
            Guard.Against.Default(value, nameof(value));
            return new ReservationDate(value);
        }

        public override string ToString()
            => this.Value.ToString(CultureInfo.InvariantCulture);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}