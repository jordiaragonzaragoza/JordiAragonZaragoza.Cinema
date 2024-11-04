namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class ExhibitionPeriod : BaseValueObject
    {
        internal ExhibitionPeriod(StartingPeriod startingPeriod, EndOfPeriod endOfPeriod)
        {
            this.StartingPeriodOnUtc = startingPeriod;
            this.EndOfPeriodOnUtc = endOfPeriod;
        }

        // Required by EF.
        private ExhibitionPeriod()
        {
        }

        public StartingPeriod StartingPeriodOnUtc { get; init; } = default!;

        public EndOfPeriod EndOfPeriodOnUtc { get; init; } = default!;

        public static ExhibitionPeriod Create(StartingPeriod startingPeriod, EndOfPeriod endOfPeriod, Runtime runtime)
        {
            ArgumentNullException.ThrowIfNull(startingPeriod, nameof(startingPeriod));
            ArgumentNullException.ThrowIfNull(endOfPeriod, nameof(endOfPeriod));
            ArgumentNullException.ThrowIfNull(runtime, nameof(runtime));

            CheckRule(new EndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule(endOfPeriod, startingPeriod));
            CheckRule(new ExhibitionPeriodMustExceedOrEqualRuntimeRule(startingPeriod, endOfPeriod, runtime));

            return new ExhibitionPeriod(startingPeriod, endOfPeriod);
        }

        public override string ToString()
            => $"StartingPeriodOnUtc: {this.StartingPeriodOnUtc} EndOfPeriodOnUtc: {this.EndOfPeriodOnUtc}";

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.StartingPeriodOnUtc;
            yield return this.EndOfPeriodOnUtc;
        }
    }
}