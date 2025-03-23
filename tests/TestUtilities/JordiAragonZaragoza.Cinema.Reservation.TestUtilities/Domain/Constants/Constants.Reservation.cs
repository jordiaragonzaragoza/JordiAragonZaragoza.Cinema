namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;

    public static partial class Constants
    {
        public static class Reservation
        {
            public static readonly ReservationId Id = new ReservationId(Guid.NewGuid());
            public static readonly UserId UserId = new UserId(Guid.NewGuid());
            public static readonly IEnumerable<SeatId> SeatIds = CreateAuditoriumUtils.Create().Seats.Select(seat => seat.Id);
            public static readonly ReservationDate ReservationDateOnUtc = ReservationDate.Create(new DateTimeOffset(2010, 01, 14, 0, 0, 0, TimeSpan.Zero));
        }
    }
}