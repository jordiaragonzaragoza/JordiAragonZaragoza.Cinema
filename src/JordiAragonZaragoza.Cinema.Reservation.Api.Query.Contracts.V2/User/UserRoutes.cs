namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User
{
    public static class UserRoutes
    {
        public const string GetUsers = $"{Base}";
        public const string GetUserReservation = $"{Base}/{{UserId}}/showtimes/{{ShowtimeId}}/reservations/{{ReservationId}}";
        public const string GetUserReservations = $"{Base}/{{UserId}}/reservations";
        private const string Base = "users";
    }
}