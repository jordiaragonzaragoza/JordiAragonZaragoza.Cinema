namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime
{
    public static class ShowtimeRoutes
    {
        public const string GetShowtimes = $"{Base}";
        public const string GetShowtime = $"{Base}/{{showtimeId}}";
        public const string GetAvailableSeats = $"{Base}/{{showtimeId}}/seats/available";
        public const string GetShowtimeReservations = $"{Base}/{{showtimeId}}/reservations";
        private const string Base = "showtimes";
    }
}