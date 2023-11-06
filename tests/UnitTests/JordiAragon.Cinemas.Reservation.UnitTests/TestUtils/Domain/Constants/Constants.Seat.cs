namespace JordiAragon.Cinemas.Reservation.UnitTests.TestUtils.Domain
{
    using System;
    using JordiAragon.Cinemas.Reservation.Auditorium.Domain;

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