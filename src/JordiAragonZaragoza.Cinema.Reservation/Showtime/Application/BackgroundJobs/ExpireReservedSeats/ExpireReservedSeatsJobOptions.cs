namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.BackgroundJobs.ExpireReservedSeats
{
    public sealed class ExpireReservedSeatsJobOptions
    {
        public const string Section = "BackgroundJobs:ExpireReservedSeatsJob";

        public int ScheduleIntervalInSeconds { get; set; }
    }
}