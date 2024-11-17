namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Rules
{
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class MinimumRowsRule : IBusinessRule
    {
        private readonly ushort minimumRowsRule;

        public MinimumRowsRule(ushort minimumRowsRule)
        {
            this.minimumRowsRule = minimumRowsRule;
        }

        public string Message => "The minimum rows value must be valid.";

        public bool IsBroken()
        {
            if (this.minimumRowsRule == default)
            {
                return true;
            }

            return false;
        }
    }
}