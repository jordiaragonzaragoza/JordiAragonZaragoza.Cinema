namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class MinimumStartingPeriodRule : IBusinessRule
    {
        private readonly DateTimeOffset minimumStartingPeriod;

        public MinimumStartingPeriodRule(DateTimeOffset minimumStartingPeriod)
        {
            this.minimumStartingPeriod = Guard.Against.Null(minimumStartingPeriod, nameof(minimumStartingPeriod));
        }

        public string Message => "The minimum starting period must be valid.";

        public bool IsBroken()
        {
            if (this.minimumStartingPeriod == default)
            {
                return true;
            }

            return false;
        }
    }
}