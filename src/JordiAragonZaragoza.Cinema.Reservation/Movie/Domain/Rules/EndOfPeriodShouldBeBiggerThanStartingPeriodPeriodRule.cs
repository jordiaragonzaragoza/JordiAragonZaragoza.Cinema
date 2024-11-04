namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules
{
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class EndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule : IBusinessRule
    {
        private readonly EndOfPeriod endOfPeriod;
        private readonly StartingPeriod startingPeriod;

        public EndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule(EndOfPeriod endOfPeriod, StartingPeriod startingPeriod)
        {
            this.endOfPeriod = Guard.Against.Null(endOfPeriod, nameof(endOfPeriod));
            this.startingPeriod = Guard.Against.Null(startingPeriod, nameof(startingPeriod));
        }

        public string Message => "The end of period must be bigger than starting period to be valid.";

        public bool IsBroken()
        {
            if (this.endOfPeriod.Value <= this.startingPeriod.Value)
            {
                return true;
            }

            return false;
        }
    }
}