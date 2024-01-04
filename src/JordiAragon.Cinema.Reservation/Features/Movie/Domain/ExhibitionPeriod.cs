namespace JordiAragon.Cinema.Reservation.Movie.Domain
{
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public class ExhibitionPeriod : BaseValueObject
    {
        private ExhibitionPeriod(StartingPeriod startingPeriod, EndOfPeriod endOfPeriod)
        {
            Guard.Against.Null(startingPeriod, nameof(startingPeriod));
            Guard.Against.Null(endOfPeriod, nameof(endOfPeriod));

            this.StartingPeriodOnUtc = startingPeriod;
            this.EndOfPeriodOnUtc = endOfPeriod;
        }

        private ExhibitionPeriod()
        {
        }

        public StartingPeriod StartingPeriodOnUtc { get; init; }

        public EndOfPeriod EndOfPeriodOnUtc { get; init; }

        public static ExhibitionPeriod Create(StartingPeriod startingPeriod, EndOfPeriod endOfPeriod)
        {
            return new ExhibitionPeriod(startingPeriod, endOfPeriod);
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