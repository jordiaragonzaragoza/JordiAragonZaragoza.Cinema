namespace JordiAragon.Cinema.Reservation.TestUtilities.Domain
{
    using System;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;

    public static partial class Constants
    {
        public static class Seat
        {
            public const short Row = 10;
            public const short SeatNumber = 10;
            public static readonly SeatId Id = SeatId.Create(Guid.NewGuid());
        }
    }
}