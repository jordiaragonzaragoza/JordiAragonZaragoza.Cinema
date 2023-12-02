namespace JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain
{
    using System;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;

    public static partial class Constants
    {
        public static class Auditorium
        {
            public static readonly AuditoriumId Id = AuditoriumId.Create(Guid.NewGuid());
            public static readonly short Rows = 10;
            public static readonly short SeatsPerRow = 10;
        }
    }
}