namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Rules
{
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class MinimumSeatsPerRowRule : IBusinessRule
    {
        private readonly ushort minimumSeatsPerRowRule;

        public MinimumSeatsPerRowRule(ushort minimumSeatsPerRowRule)
        {
            this.minimumSeatsPerRowRule = minimumSeatsPerRowRule;
        }

        public string Message => "The minimum seat per row value must be valid.";

        public bool IsBroken()
        {
            if (this.minimumSeatsPerRowRule == default)
            {
                return true;
            }

            return false;
        }
    }
}