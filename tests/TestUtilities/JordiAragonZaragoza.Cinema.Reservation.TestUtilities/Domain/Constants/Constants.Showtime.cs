namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;

    public static partial class Constants
    {
        public static class Showtime
        {
            public static readonly ShowtimeId Id = new(new Guid("89b073a7-cfcf-4f2a-b01b-4c7f71a0563b"));
            public static readonly MovieId MovieId = Constants.Movie.Id;
            public static readonly SessionDate SessionDateOnUtc = SessionDate.Create(DateTimeOffset.UtcNow.AddYears(1));
            public static readonly AuditoriumId AuditoriumId = Constants.Auditorium.Id;
        }
    }
}