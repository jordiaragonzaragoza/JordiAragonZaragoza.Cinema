namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using System.Collections.Generic;
    using System.Globalization;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class SeatsPerRow : BaseValueObject
    {
        private SeatsPerRow(ushort value)
        {
            // This also checks seatsPerRow value must be greater than zero.
            Guard.Against.Default(value, nameof(value));

            this.Value = value;
        }

        public ushort Value { get; init; }

        public static implicit operator ushort(SeatsPerRow seatsPerRow)
        {
            Guard.Against.Null(seatsPerRow, nameof(seatsPerRow));

            return seatsPerRow.Value;
        }

        public static explicit operator SeatsPerRow(ushort value)
        {
            return new SeatsPerRow(value);
        }

        public static ushort FromSeatsPerRow(SeatsPerRow seatsPerRow)
        {
            return seatsPerRow;
        }

        public static SeatsPerRow ToSeatsPerRow(ushort value)
        {
            return (SeatsPerRow)value;
        }

        public static SeatsPerRow Create(ushort value)
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