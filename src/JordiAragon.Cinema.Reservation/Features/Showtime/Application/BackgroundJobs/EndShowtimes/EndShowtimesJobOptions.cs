namespace JordiAragon.Cinema.Reservation.Showtime.Application.BackgroundJobs.EndShowtimes
{
    public class EndShowtimesJobOptions
    {
        public const string Section = "BackgroundJobs:EndShowtimesJob";

        public int ScheduleIntervalInSeconds { get; set; }
    }
}