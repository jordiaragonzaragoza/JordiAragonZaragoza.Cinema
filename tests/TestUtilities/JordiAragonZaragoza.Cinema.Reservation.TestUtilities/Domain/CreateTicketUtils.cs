namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain
{
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;

    public static class CreateTicketUtils
    {
        public static Ticket Create()
            => new Ticket(
                Constants.Ticket.Id,
                Constants.Ticket.UserId,
                Constants.Ticket.SeatIds,
                Constants.Ticket.ReservationDateOnUtc);
    }
}