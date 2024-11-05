namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class MinimumRuntimeRule : IBusinessRule
    {
        private readonly TimeSpan minimumRuntime;

        public MinimumRuntimeRule(TimeSpan minimumRuntime)
        {
            this.minimumRuntime = Guard.Against.Null(minimumRuntime, nameof(minimumRuntime));
        }

        public string Message => "The runtime must be greater than zero to be valid.";

        public bool IsBroken()
        {
            if (this.minimumRuntime <= TimeSpan.Zero)
            {
                return true;
            }

            return false;
        }
    }
}