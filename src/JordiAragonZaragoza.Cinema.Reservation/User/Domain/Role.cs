namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class Role : BaseValueObject
    {
        internal Role(string value)
            => this.Value = value;

        public string Value { get; init; }

        public static implicit operator string(Role role)
        {
            ArgumentNullException.ThrowIfNull(role, nameof(role));

            return role.Value;
        }

        public static string FromRole(Role role)
            => role;

        public static Role Create(string value)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));

            return new Role(value);
        }

        public override string ToString()
            => this.Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}