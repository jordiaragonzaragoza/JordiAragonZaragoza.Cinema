namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1.Auditorium
{
    public static class AuditoriumRoutes
    {
        public const string GetAuditoriums = $"{Base}";
        public const string GetShowtimes = $"{Base}/{{AuditoriumId}}/showtimes";
        public const string GetAvailableSeats = $"{Base}/{{AuditoriumId}}/showtimes/{{ShowtimeId}}/seats/available";
        private const string Base = "auditoriums";
    }
}