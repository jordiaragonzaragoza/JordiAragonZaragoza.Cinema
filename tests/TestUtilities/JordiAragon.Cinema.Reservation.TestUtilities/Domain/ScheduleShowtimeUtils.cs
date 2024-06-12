namespace JordiAragon.Cinema.Reservation.TestUtilities.Domain
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