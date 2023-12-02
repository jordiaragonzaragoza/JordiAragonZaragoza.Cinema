namespace JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;

    public static partial class Constants
    {
        public static class Ticket
        {
            public static readonly TicketId Id = TicketId.Create(Guid.NewGuid());
            public static readonly IEnumerable<SeatId> SeatIds = CreateAuditoriumUtils.Create().Seats.Select(seat => seat.Id);
            public static readonly DateTime CreatedTimeOnUtc = new(2010, 01, 14, 0, 0, 0, DateTimeKind.Utc);
        }
    }
}