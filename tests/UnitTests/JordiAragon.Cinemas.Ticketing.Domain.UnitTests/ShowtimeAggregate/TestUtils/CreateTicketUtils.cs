namespace JordiAragon.Cinemas.Ticketing.Domain.UnitTests.TicketAggregate.TestUtils
{
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate;
    using JordiAragon.Cinemas.Ticketing.Domain.UnitTests.TestUtils.Constants;

    public static class CreateTicketUtils
    {
        public static Ticket Create()
            => Ticket.Create(
                Constants.Ticket.Id,
                Constants.Ticket.SeatIds,
                Constants.Ticket.CreatedTimeOnUtc);
    }
}