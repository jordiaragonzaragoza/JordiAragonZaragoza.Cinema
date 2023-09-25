namespace JordiAragon.Cinemas.Ticketing.Domain.UnitTests.TestUtils.Constants
{
    using System;
    using JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate;

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