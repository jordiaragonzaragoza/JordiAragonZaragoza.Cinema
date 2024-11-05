namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Rules
{
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class MinimumSeatNumberRule : IBusinessRule
    {
        private readonly ushort minimumSeatNumberRule;

        public MinimumSeatNumberRule(ushort minimumSeatNumberRule)
        {
            this.minimumSeatNumberRule = minimumSeatNumberRule;
        }

        public string Message => "The minimum seat number value must be valid.";

        public bool IsBroken()
        {
            if (this.minimumSeatNumberRule == default)
            {
                return true;
            }

            return false;
        }
    }
}