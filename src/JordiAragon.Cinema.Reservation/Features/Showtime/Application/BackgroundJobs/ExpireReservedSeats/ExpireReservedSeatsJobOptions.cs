namespace JordiAragon.Cinema.Reservation.Showtime.Application.BackgroundJobs.ExpireReservedSeats
{
    public class ExpireReservedSeatsJobOptions
    {
        public const string Section = "BackgroundJobs:ExpireReservedSeatsJob";

        public int ScheduleIntervalInSeconds { get; set; }
    }
}