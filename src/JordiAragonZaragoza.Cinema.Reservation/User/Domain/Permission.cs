namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class Permission : BaseValueObject
    {
        internal Permission(string value)
            => this.Value = value;

        public string Value { get; init; }

        public static implicit operator string(Permission role)
        {
            ArgumentNullException.ThrowIfNull(role, nameof(role));

            return role.Value;
        }

        public static string FromPermission(Permission role)
            => role;

        public static Permission Create(string value)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));

            return new Permission(value);
        }

        public override string ToString()
            => this.Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}