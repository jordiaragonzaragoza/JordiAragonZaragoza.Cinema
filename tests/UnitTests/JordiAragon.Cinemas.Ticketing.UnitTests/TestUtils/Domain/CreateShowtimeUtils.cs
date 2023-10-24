namespace JordiAragon.Cinemas.Ticketing.UnitTests.TestUtils.Domain
{
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain;

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