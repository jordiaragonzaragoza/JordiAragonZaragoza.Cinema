namespace JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants
{
    using System;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;

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