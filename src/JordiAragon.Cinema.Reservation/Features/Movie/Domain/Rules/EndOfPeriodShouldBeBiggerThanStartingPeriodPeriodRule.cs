namespace JordiAragon.Cinema.Reservation.Movie.Domain.Rules
{
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class EndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule : IBusinessRule
    {
        private readonly ExhibitionPeriod exhibitionPeriod;

        public EndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule(ExhibitionPeriod exhibitionPeriod)
        {
            this.exhibitionPeriod = Guard.Against.Null(exhibitionPeriod, nameof(exhibitionPeriod));
        }

        public string Message => "The end of period must be bigger than starting period to be valid.";

        public bool IsBroken()
        {
            if (this.exhibitionPeriod.EndOfPeriodOnUtc.Value > this.exhibitionPeriod.StartingPeriodOnUtc.Value)
            {
                return false;
            }

            return true;
        }
    }
}