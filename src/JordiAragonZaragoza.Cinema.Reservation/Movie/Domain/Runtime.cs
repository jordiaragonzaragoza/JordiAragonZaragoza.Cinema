namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class Runtime : BaseValueObject
    {
        internal Runtime(TimeSpan value)
            => this.Value = value;

        public TimeSpan Value { get; init; }

        public static implicit operator TimeSpan(Runtime runtime)
        {
            ArgumentNullException.ThrowIfNull(runtime, nameof(runtime));

            return runtime.Value;
        }

        public static TimeSpan FromRuntime(Runtime runtime)
            => runtime;

        public static Runtime Create(TimeSpan value)
        {
            Guard.Against.Default(value, nameof(value));

            return new Runtime(value);
        }

        public override string ToString()
            => this.Value.ToString();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}