namespace JordiAragon.Cinema.Domain.UnitTests.TicketAggregate.TestUtils
{
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants;

    public static class CreateTicketUtils
    {
        public static Ticket Create()
            => Ticket.Create(
                Constants.Ticket.Id,
                Constants.Ticket.SeatIds,
                Constants.Ticket.CreatedTimeOnUtc);
    }
}