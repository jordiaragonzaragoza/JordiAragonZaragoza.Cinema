namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class MinimumEndOfPeriodRule : IBusinessRule
    {
        private readonly DateTimeOffset minimumEndOfPeriod;

        public MinimumEndOfPeriodRule(DateTimeOffset minimumEndOfPeriod)
        {
            this.minimumEndOfPeriod = minimumEndOfPeriod;
        }

        public string Message => "The minimum end of period must be valid.";

        public bool IsBroken()
        {
            if (this.minimumEndOfPeriod == default)
            {
                return true;
            }

            return false;
        }
    }
}