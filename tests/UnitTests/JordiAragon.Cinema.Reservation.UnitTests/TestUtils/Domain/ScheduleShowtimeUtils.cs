namespace JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain
{
    using JordiAragon.Cinema.Reservation.Showtime.Domain;

    public static class ScheduleShowtimeUtils
    {
        public static Showtime Schedule()
            => Showtime.Schedule(
                Constants.Showtime.Id,
                Constants.Showtime.MovieId,
                Constants.Showtime.SessionDateOnUtc,
                Constants.Showtime.AuditoriumId);
    }
}