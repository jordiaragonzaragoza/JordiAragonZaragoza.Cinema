namespace JordiAragon.Cinema.Reservation.Auditorium.Domain
{
    using System.Collections.Generic;
    using System.Globalization;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class SeatNumber : BaseValueObject
    {
        private SeatNumber(ushort value)
        {
            // This also checks seatNumber value must be greater than zero.
            Guard.Against.Default(value, nameof(value));

            this.Value = value;
        }

        public ushort Value { get; init; }

        public static implicit operator ushort(SeatNumber seatNumber)
        {
            Guard.Against.Null(seatNumber, nameof(seatNumber));

            return seatNumber.Value;
        }

        public static explicit operator SeatNumber(ushort value)
        {
            return new SeatNumber(value);
        }

        public static ushort FromSeatNumber(SeatNumber seatNumber)
        {
            return seatNumber;
        }

        public static SeatNumber ToSeatNumber(ushort value)
        {
            return (SeatNumber)value;
        }

        public static SeatNumber Create(ushort value)
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