namespace JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain
{
    using JordiAragon.Cinema.Reservation.Showtime.Domain;

    public static class CreateShowtimeUtils
    {
        public static Showtime Create()
            => Showtime.Create(
                Constants.Showtime.Id,
                Constants.Showtime.MovieId,
                Constants.Showtime.SessionDateOnUtc,
                Constants.Showtime.AuditoriumId);
    }
}