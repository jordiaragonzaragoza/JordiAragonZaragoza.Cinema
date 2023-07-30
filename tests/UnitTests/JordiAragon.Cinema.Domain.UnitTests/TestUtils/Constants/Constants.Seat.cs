namespace JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants
{
    using System;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;

    public static partial class Constants
    {
        public static class Seat
        {
            public static readonly SeatId Id = SeatId.Create(Guid.NewGuid());
        }
    }
}