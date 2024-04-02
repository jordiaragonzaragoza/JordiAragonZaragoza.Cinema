namespace JordiAragon.Cinema.Reservation.Showtime.Application.BackgroundJobs.EndShowtimes
{
    public sealed class EndShowtimesJobOptions
    {
        public const string Section = "BackgroundJobs:EndShowtimesJob";

        public int ScheduleIntervalInSeconds { get; set; }
    }
}