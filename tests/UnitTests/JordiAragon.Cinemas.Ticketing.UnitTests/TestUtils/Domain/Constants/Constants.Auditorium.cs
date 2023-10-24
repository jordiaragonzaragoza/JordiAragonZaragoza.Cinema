namespace JordiAragon.Cinemas.Ticketing.UnitTests.TestUtils.Domain
{
    using System;
    using JordiAragon.Cinemas.Ticketing.Auditorium.Domain;

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