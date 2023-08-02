namespace JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants
{
    using System;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;

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