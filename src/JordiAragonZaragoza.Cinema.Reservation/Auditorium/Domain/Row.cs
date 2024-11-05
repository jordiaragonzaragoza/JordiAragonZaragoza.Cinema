namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Rules;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class Row : BaseValueObject
    {
        internal Row(ushort value)
            => this.Value = value;

        public ushort Value { get; init; }

        public static implicit operator ushort(Row row)
        {
            ArgumentNullException.ThrowIfNull(row, nameof(row));

            return row.Value;
        }

        public static ushort FromRow(Row row)
        {
            return row;
        }

        public static Row Create(ushort value)
        {
            CheckRule(new MinimumRowRule(value));

            return new Row(value);
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