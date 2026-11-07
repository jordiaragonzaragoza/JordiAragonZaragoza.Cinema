namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class ExhibitionPeriodMustExceedOrEqualRuntimeRule : IBusinessRule
    {
        private readonly StartingPeriod startingPeriod;
        private readonly EndOfPeriod endOfPeriod;
        private readonly Runtime runtime;

        public ExhibitionPeriodMustExceedOrEqualRuntimeRule(StartingPeriod startingPeriod, EndOfPeriod endOfPeriod, Runtime runtime)
        {
            this.startingPeriod = startingPeriod ?? throw new ArgumentNullException(nameof(startingPeriod));
            this.endOfPeriod = endOfPeriod ?? throw new ArgumentNullException(nameof(endOfPeriod));
            this.runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
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