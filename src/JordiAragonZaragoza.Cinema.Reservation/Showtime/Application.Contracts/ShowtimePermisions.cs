namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts
{
    public static class ShowtimePermisions
    {
        public const string Cancel = "cancel:showtime";
        public const string End = "end:showtime";
        public const string ReserveSeats = "reserveSeats:showtime";
        public const string ScheduleShowtime = "scheduleShowtime:showtime";
        public const string GetAvailableSeats = "getAvailableSeats:showtime";
        public const string GetShowtime = "getShowtime:showtime";
        public const string GetShowtimes = "getShowtimes:showtime";
        public const string GetShowtimeReservation = "getShowtimeReservation:showtime";
        public const string GetShowtimeReservations = "getShowtimeReservations:showtime";
    }
}
