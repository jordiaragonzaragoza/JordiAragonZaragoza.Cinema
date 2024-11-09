namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class Title : BaseValueObject
    {
        internal Title(string value)
            => this.Value = value;

        public string Value { get; init; }

        public static implicit operator string(Title title)
        {
            ArgumentNullException.ThrowIfNull(title, nameof(title));

            return title.Value;
        }

        public static string FromTitle(Title title)
            => title;

        public static Title Create(string value)
        {
            Guard.Against.NullOrWhiteSpace(value, nameof(value));

            return new Title(value);
        }

        public override string ToString()
            => this.Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}