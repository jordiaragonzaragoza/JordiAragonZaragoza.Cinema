namespace JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants
{
    using System;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;

    public static partial class Constants
    {
        public static class Ticket
        {
            public static readonly TicketId Id = TicketId.Create(Guid.NewGuid());
        }
    }
}