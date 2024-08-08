namespace JordiAragon.Cinema.Reservation.Movie.Domain.Rules
{
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class ExhibitionPeriodMustExceedOrEqualRuntimeRule : IBusinessRule
    {
        private readonly ExhibitionPeriod exhibitionPeriod;
        private readonly Runtime runtime;

        public ExhibitionPeriodMustExceedOrEqualRuntimeRule(ExhibitionPeriod exhibitionPeriod, Runtime runtime)
        {
            this.exhibitionPeriod = Guard.Against.Null(exhibitionPeriod, nameof(exhibitionPeriod));
            this.runtime = Guard.Against.Default(runtime, nameof(runtime));
        }

        public string Message => "The exhibition period must exceed or equal the runtime to be valid.";

        public bool IsBroken()
        {
            // TODO: Review check rule using implicit operators.
            if (this.exhibitionPeriod.EndOfPeriodOnUtc.Value - this.exhibitionPeriod.StartingPeriodOnUtc.Value >= this.runtime.Value)
            {
                return false;
            }

            return true;
        }
    }
}