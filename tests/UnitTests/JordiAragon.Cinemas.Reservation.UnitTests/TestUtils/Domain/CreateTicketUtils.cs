namespace JordiAragon.Cinemas.Reservation.UnitTests.TestUtils.Domain
{
    using JordiAragon.Cinemas.Reservation.Showtime.Domain;

    public static class CreateTicketUtils
    {
        public static Ticket Create()
            => Ticket.Create(
                Constants.Ticket.Id,
                Constants.Ticket.SeatIds,
                Constants.Ticket.CreatedTimeOnUtc);
    }
}