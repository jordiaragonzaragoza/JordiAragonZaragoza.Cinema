namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1.Auditorium
{
    public static class AuditoriumRoutes
    {
        public const string GetAuditoriums = $"{Base}";
        public const string GetShowtimes = $"{Base}/{{auditoriumId}}/showtimes";
        public const string GetAvailableSeats = $"{Base}/{{auditoriumId}}/showtimes/{{showtimeId}}/seats/available";
        private const string Base = $"{Routes.ApiBase}/auditoriums";
    }
}