namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Rules
{
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class OnlyPossibleToPurchaseOncePerReservationRule : IBusinessRule
    {
        private readonly Reservation reservation;

        public OnlyPossibleToPurchaseOncePerReservationRule(Reservation reservation)
        {
            this.reservation = Guard.Against.Null(reservation, nameof(reservation));
        }

        public string Message => "Only possible to purchase once per reservation.";

        public bool IsBroken() => this.reservation.IsPurchased;
    }
}