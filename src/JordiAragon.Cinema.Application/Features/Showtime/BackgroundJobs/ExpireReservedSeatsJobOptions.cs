namespace JordiAragon.Cinema.Application.Features.Auditorium.Ticket.BackgroundJobs
{
    public class ExpireReservedSeatsJobOptions
    {
        public const string Section = "BackgroundJobs:ExpireReservedSeatsJob";

        public int ScheduleIntervalInSeconds { get; set; }
    }
}