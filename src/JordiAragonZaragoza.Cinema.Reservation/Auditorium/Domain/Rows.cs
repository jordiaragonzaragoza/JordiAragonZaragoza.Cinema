namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using System.Collections.Generic;
    using System.Globalization;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class Rows : BaseValueObject
    {
        private Rows(ushort value)
        {
            // This also checks rows value must be greater than zero.
            Guard.Against.Default(value, nameof(value));

            this.Value = value;
        }

        public ushort Value { get; init; }

        public static implicit operator ushort(Rows rows)
        {
            Guard.Against.Null(rows, nameof(rows));

            return rows.Value;
        }

        public static explicit operator Rows(ushort value)
        {
            return new Rows(value);
        }

        public static ushort FromRows(Rows rows)
        {
            return rows;
        }

        public static Rows ToRows(ushort value)
        {
            return (Rows)value;
        }

        public static Rows Create(ushort value)
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