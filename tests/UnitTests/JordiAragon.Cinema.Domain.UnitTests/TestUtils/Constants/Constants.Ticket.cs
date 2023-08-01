namespace JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.Cinema.Domain.UnitTests.Features.Auditorium.TestUtils;

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