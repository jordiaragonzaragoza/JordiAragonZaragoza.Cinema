namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime
{
    public static class ShowtimeRoutes
    {
        public const string GetShowtimes = $"{Base}";
        public const string GetShowtime = $"{Base}/{{ShowtimeId}}";
        public const string GetAvailableSeats = $"{Base}/{{ShowtimeId}}/seats/available";
        public const string GetShowtimeReservations = $"{Base}/{{ShowtimeId}}/reservations";
        private const string Base = "showtimes";
    }
}