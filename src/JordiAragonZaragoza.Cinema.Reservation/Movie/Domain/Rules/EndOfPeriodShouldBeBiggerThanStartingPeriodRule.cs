namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class EndOfPeriodShouldBeBiggerThanStartingPeriodRule : IBusinessRule
    {
        private readonly EndOfPeriod endOfPeriod;
        private readonly StartingPeriod startingPeriod;

        public EndOfPeriodShouldBeBiggerThanStartingPeriodRule(EndOfPeriod endOfPeriod, StartingPeriod startingPeriod)
        {
            this.endOfPeriod = endOfPeriod ?? throw new ArgumentNullException(nameof(endOfPeriod));
            this.startingPeriod = startingPeriod ?? throw new ArgumentNullException(nameof(startingPeriod));
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