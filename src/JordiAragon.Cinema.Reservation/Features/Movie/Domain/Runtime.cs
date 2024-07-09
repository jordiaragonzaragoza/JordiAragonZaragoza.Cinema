namespace JordiAragon.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class Runtime : BaseValueObject
    {
        private Runtime(TimeSpan value)
        {
            Guard.Against.Default(value, nameof(value));

            this.Value = value;
        }

        public TimeSpan Value { get; init; }

        public static implicit operator TimeSpan(Runtime runtime)
        {
            Guard.Against.Null(runtime, nameof(runtime));

            return runtime.Value;
        }

        public static explicit operator Runtime(TimeSpan value)
        {
            return new Runtime(value);
        }

        public static Runtime Create(TimeSpan value)
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