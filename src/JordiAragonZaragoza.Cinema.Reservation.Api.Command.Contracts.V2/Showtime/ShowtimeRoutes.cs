namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime
{
    public static class ShowtimeRoutes
    {
        public const string PurchaseReservation = $"{Base}/{{showtimeId}}/reservations/{{reservationId}}/purchase";

        public const string ReserveSeats = $"{Base}/{{showtimeId}}/reservations/{{reservationId}}";

        public const string ScheduleShowtime = $"{Base}/{{showtimeId}}";

        public const string CancelShowtime = $"{Base}/{{showtimeId}}";

        private const string Base = "showtimes";
    }
}