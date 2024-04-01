namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Outbox
{
    public sealed class ProcessOutboxMessagesJobOptions
    {
        public const string Section = "BackgroundJobs:ProcessOutboxMessagesJob";

        public int ScheduleIntervalInSeconds { get; set; }
    }
}