namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Rules
{
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class MinimumRowRule : IBusinessRule
    {
        private readonly ushort minimumRowRule;

        public MinimumRowRule(ushort minimumRowRule)
        {
            this.minimumRowRule = minimumRowRule;
        }

        public string Message => "The minimum row value must be valid.";

        public bool IsBroken()
        {
            if (this.minimumRowRule == default)
            {
                return true;
            }

            return false;
        }
    }
}