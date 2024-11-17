namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Rules;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class SeatsPerRow : BaseValueObject
    {
        internal SeatsPerRow(ushort value)
            => this.Value = value;

        public ushort Value { get; init; }

        public static implicit operator ushort(SeatsPerRow seatsPerRow)
        {
            ArgumentNullException.ThrowIfNull(seatsPerRow, nameof(seatsPerRow));

            return seatsPerRow.Value;
        }

        public static ushort FromSeatsPerRow(SeatsPerRow seatsPerRow)
        {
            return seatsPerRow;
        }

        public static SeatsPerRow Create(ushort value)
        {
            CheckRule(new MinimumSeatsPerRowRule(value));

            return new SeatsPerRow(value);
        }

        public override string ToString()
         => this.Value.ToString(CultureInfo.InvariantCulture);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}