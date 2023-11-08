namespace JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain
{
    using JordiAragon.Cinema.Reservation.Showtime.Domain;

    public static class CreateTicketUtils
    {
        public static Ticket Create()
            => Ticket.Create(
                Constants.Ticket.Id,
                Constants.Ticket.SeatIds,
                Constants.Ticket.CreatedTimeOnUtc);
    }
}