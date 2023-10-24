namespace JordiAragon.Cinemas.Ticketing.UnitTests.TestUtils.Domain
{
    using System;
    using JordiAragon.Cinemas.Ticketing.Auditorium.Domain;
    using JordiAragon.Cinemas.Ticketing.Movie.Domain;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain;

    public static partial class Constants
    {
        public static class Showtime
        {
            public static readonly ShowtimeId Id = ShowtimeId.Create(Guid.NewGuid());
            public static readonly MovieId MovieId = MovieId.Create(Guid.NewGuid());
            public static readonly DateTime SessionDateOnUtc = DateTime.UtcNow.AddYears(1);
            public static readonly AuditoriumId AuditoriumId = AuditoriumId.Create(Guid.NewGuid());
        }
    }
}