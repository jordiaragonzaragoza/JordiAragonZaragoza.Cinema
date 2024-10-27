namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain
{
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class ExhibitionPeriod : BaseValueObject
    {
        private ExhibitionPeriod(StartingPeriod startingPeriod, EndOfPeriod endOfPeriod, Runtime runtime)
        {
            Guard.Against.Null(startingPeriod, nameof(startingPeriod));
            Guard.Against.Null(endOfPeriod, nameof(endOfPeriod));
            Guard.Against.Null(runtime, nameof(runtime));

            this.StartingPeriodOnUtc = startingPeriod;
            this.EndOfPeriodOnUtc = endOfPeriod;

            ExhibitionPeriod.CheckRule(new EndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule(this));
            ExhibitionPeriod.CheckRule(new ExhibitionPeriodMustExceedOrEqualRuntimeRule(this, runtime));
        }

        private ExhibitionPeriod()
        {
        }

        public StartingPeriod StartingPeriodOnUtc { get; init; } = default!;

        public EndOfPeriod EndOfPeriodOnUtc { get; init; } = default!;

        public static ExhibitionPeriod Create(StartingPeriod startingPeriod, EndOfPeriod endOfPeriod, Runtime runtime)
        {
            return new ExhibitionPeriod(startingPeriod, endOfPeriod, runtime);
        }

        public override string ToString()
        {
            return $"StartingPeriodOnUtc: {this.StartingPeriodOnUtc} EndOfPeriodOnUtc: {this.EndOfPeriodOnUtc}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.StartingPeriodOnUtc;
            yield return this.EndOfPeriodOnUtc;
        }
    }
}