namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class Name : BaseValueObject
    {
        internal Name(string value)
            => this.Value = value;

        public string Value { get; init; }

        public static implicit operator string(Name name)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));

            return name.Value;
        }

        public static string FromName(Name name)
            => name;

        public static Name Create(string value)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(value));

            return new Name(value);
        }

        public override string ToString()
            => this.Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}