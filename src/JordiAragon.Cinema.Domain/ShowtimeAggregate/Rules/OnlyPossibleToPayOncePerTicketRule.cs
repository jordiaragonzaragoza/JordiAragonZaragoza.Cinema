namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Rules
{
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class OnlyPossibleToPayOncePerTicketRule : IBusinessRule
    {
        private readonly Ticket reservation;

        public OnlyPossibleToPayOncePerTicketRule(Ticket reservation)
        {
            this.reservation = Guard.Against.Null(reservation, nameof(reservation));
        }

        public string Message => "Only possible to pay once per reservation.";

        public bool IsBroken() => this.reservation.IsPaid;
    }
}