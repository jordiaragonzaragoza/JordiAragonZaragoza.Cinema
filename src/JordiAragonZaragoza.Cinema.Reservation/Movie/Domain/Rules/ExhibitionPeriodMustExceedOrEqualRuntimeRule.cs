namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules
{
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class ExhibitionPeriodMustExceedOrEqualRuntimeRule : IBusinessRule
    {
        private readonly StartingPeriod startingPeriod;
        private readonly EndOfPeriod endOfPeriod;
        private readonly Runtime runtime;

        public ExhibitionPeriodMustExceedOrEqualRuntimeRule(StartingPeriod startingPeriod, EndOfPeriod endOfPeriod, Runtime runtime)
        {
            this.startingPeriod = Guard.Against.Null(startingPeriod, nameof(startingPeriod));
            this.endOfPeriod = Guard.Against.Null(endOfPeriod, nameof(endOfPeriod));
            this.runtime = Guard.Against.Null(runtime, nameof(runtime));
        }

        public string Message => "The exhibition period must exceed or equal the runtime to be valid.";

        public bool IsBroken()
        {
            // TODO: Review check rule using implicit operators.
            if (this.endOfPeriod.Value - this.startingPeriod.Value < this.runtime.Value)
            {
                return true;
            }

            return false;
        }
    }
}