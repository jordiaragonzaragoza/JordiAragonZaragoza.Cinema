namespace JordiAragon.Cinemas.Ticketing.UnitTests.TestUtils.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragon.Cinemas.Ticketing.Auditorium.Domain;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain;

    public static partial class Constants
    {
        public static class Ticket
        {
            public static readonly TicketId Id = TicketId.Create(Guid.NewGuid());
            public static readonly IEnumerable<SeatId> SeatIds = CreateAuditoriumUtils.Create().Seats.Select(seat => seat.Id);
            public static readonly DateTime CreatedTimeOnUtc = new(2010, 01, 14);
        }
    }
}