namespace JordiAragon.Cinemas.Ticketing.Domain.UnitTests.TestUtils.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate;
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate;
    using JordiAragon.Cinemas.Ticketing.Domain.UnitTests.Features.Auditorium.TestUtils;

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