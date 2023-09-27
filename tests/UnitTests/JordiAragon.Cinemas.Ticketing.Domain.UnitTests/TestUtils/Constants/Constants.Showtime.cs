namespace JordiAragon.Cinemas.Ticketing.Domain.UnitTests.TestUtils.Constants
{
    using System;
    using JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate;
    using JordiAragon.Cinemas.Ticketing.Domain.MovieAggregate;
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate;

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