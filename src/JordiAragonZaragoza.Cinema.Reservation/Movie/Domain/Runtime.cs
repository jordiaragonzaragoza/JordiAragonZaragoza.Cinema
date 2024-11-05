namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules;
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
            CheckRule(new MinimumRuntimeRule(value));

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