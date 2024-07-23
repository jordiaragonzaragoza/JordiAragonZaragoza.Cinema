namespace JordiAragon.Cinema.Reservation.Auditorium.Domain
{
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class Row : BaseValueObject
    {
        private Row(ushort value)
        {
            // This also checks row value must be greater than zero.
            Guard.Against.Default(value, nameof(value));

            this.Value = value;
        }

        public ushort Value { get; init; }

        public static implicit operator ushort(Row row)
        {
            Guard.Against.Null(row, nameof(row));

            return row.Value;
        }

        public static explicit operator Row(ushort value)
        {
            return new Row(value);
        }

        public static Row Create(ushort value)
            => new(value);

        public override string ToString()
        {
            return this.Value.ToString();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}