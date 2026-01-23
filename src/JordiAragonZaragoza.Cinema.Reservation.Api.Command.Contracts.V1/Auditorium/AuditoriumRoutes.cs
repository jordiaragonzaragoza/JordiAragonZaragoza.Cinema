namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium
{
    public static class AuditoriumRoutes
    {
        public const string PurchaseReservation = $"{Base}/{{auditoriumId}}/showtimes/{{showtimeId}}/reservations/{{reservationId}}/purchase";

        public const string ReserveSeats = $"{Base}/{{auditoriumId}}/showtimes/{{showtimeId}}/reservations";

        public const string ScheduleShowtime = $"{Base}/{{auditoriumId}}/showtimes";

        private const string Base = $"auditoriums";
    }
}