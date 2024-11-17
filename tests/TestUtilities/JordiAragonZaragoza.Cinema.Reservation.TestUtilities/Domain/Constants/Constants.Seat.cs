namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;

    public static partial class Constants
    {
        public static class Seat
        {
            public static readonly Row Row = Row.Create(10);
            public static readonly SeatNumber SeatNumber = SeatNumber.Create(10);
            public static readonly SeatId Id = new SeatId(Guid.NewGuid());
        }
    }
}