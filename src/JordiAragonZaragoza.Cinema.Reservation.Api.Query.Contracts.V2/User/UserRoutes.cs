namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User
{
    public static class UserRoutes
    {
        public const string GetUsers = $"{Base}";
        public const string GetUserReservation = $"{Base}/{{userId}}/showtimes/{{showtimeId}}/reservations/{{reservationId}}";
        public const string GetUserReservations = $"{Base}/{{userId}}/reservations";
        private const string Base = $"{Routes.ApiBase}/users";
    }
}