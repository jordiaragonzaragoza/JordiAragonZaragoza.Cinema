namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Outbox
{
    public class ProcessOutboxMessagesJobOptions
    {
        public const string Section = "BackgroundJobs:ProcessOutboxMessagesJob";

        public int ScheduleIntervalInSeconds { get; set; }
    }
}