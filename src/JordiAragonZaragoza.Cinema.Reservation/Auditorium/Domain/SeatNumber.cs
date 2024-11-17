namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Rules;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class SeatNumber : BaseValueObject
    {
        internal SeatNumber(ushort value)
            => this.Value = value;

        public ushort Value { get; init; }

        public static implicit operator ushort(SeatNumber seatNumber)
        {
            ArgumentNullException.ThrowIfNull(seatNumber, nameof(seatNumber));

            return seatNumber.Value;
        }

        public static ushort FromSeatNumber(SeatNumber seatNumber)
        {
            return seatNumber;
        }

        public static SeatNumber Create(ushort value)
        {
            CheckRule(new MinimumSeatNumberRule(value));
            return new SeatNumber(value);
        }

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