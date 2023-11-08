namespace JordiAragon.Cinema.Reservation.Showtime.Application.BackgroundJobs
{
    public class ExpireReservedSeatsJobOptions
    {
        public const string Section = "BackgroundJobs:ExpireReservedSeatsJob";

        public int ScheduleIntervalInSeconds { get; set; }
    }
}