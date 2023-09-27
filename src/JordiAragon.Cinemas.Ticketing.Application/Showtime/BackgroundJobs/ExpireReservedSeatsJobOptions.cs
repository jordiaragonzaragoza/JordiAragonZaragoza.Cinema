namespace JordiAragon.Cinemas.Ticketing.Application.Showtime.BackgroundJobs
{
    public class ExpireReservedSeatsJobOptions
    {
        public const string Section = "BackgroundJobs:ExpireReservedSeatsJob";

        public int ScheduleIntervalInSeconds { get; set; }
    }
}