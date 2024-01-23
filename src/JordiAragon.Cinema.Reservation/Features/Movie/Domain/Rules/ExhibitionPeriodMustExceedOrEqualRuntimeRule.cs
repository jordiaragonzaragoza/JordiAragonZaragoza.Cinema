namespace JordiAragon.Cinema.Reservation.Movie.Domain.Rules
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class ExhibitionPeriodMustExceedOrEqualRuntimeRule : IBusinessRule
    {
        private readonly ExhibitionPeriod exhibitionPeriod;
        private readonly TimeSpan runtime;

        public ExhibitionPeriodMustExceedOrEqualRuntimeRule(ExhibitionPeriod exhibitionPeriod, TimeSpan runtime)
        {
            this.exhibitionPeriod = Guard.Against.Null(exhibitionPeriod, nameof(exhibitionPeriod));
            this.runtime = Guard.Against.Default(runtime, nameof(runtime));
        }

        public string Message => "The exhibition period must exceed or equal the runtime to be valid.";

        public bool IsBroken()
        {
            if (this.exhibitionPeriod.EndOfPeriodOnUtc.Value - this.exhibitionPeriod.StartingPeriodOnUtc.Value >= this.runtime)
            {
                return false;
            }

            return true;
        }
    }
}