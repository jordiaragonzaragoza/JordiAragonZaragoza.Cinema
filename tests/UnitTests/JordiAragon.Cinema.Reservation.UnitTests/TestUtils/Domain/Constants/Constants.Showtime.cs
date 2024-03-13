namespace JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain
{
    using System;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;

    public static partial class Constants
    {
        public static class Showtime
        {
            public static readonly ShowtimeId Id = ShowtimeId.Create(Guid.NewGuid());
            public static readonly MovieId MovieId = MovieId.Create(Guid.NewGuid());
            public static readonly DateTimeOffset SessionDateOnUtc = DateTimeOffset.UtcNow.AddYears(1);
            public static readonly AuditoriumId AuditoriumId = AuditoriumId.Create(Guid.NewGuid());
        }
    }
}