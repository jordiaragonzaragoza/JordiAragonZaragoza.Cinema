namespace JordiAragon.Cinema.Reservation.Showtime.Domain.Rules
{
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class OnlyPossibleToPurchaseOncePerTicketRule : IBusinessRule
    {
        private readonly Ticket ticket;

        public OnlyPossibleToPurchaseOncePerTicketRule(Ticket reservation)
        {
            this.ticket = Guard.Against.Null(reservation, nameof(reservation));
        }

        public string Message => "Only possible to purchase once per ticket.";

        public bool IsBroken() => this.ticket.IsPurchased;
    }
}