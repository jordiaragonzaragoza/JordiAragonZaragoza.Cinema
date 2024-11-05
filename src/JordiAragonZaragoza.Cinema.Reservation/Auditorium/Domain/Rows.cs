namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Rules;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class Rows : BaseValueObject
    {
        internal Rows(ushort value)
            => this.Value = value;

        public ushort Value { get; init; }

        public static implicit operator ushort(Rows rows)
        {
            ArgumentNullException.ThrowIfNull(rows, nameof(rows));

            return rows.Value;
        }

        public static ushort FromRows(Rows rows)
        {
            return rows;
        }

        public static Rows Create(ushort value)
        {
            CheckRule(new MinimumRowsRule(value));

            return new Rows(value);
        }

        public override string ToString()
            => this.Value.ToString(CultureInfo.InvariantCulture);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}