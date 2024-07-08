namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.Outbox
{
    public sealed class ProcessOutboxMessagesJobOptions
    {
        public const string Section = "BackgroundJobs:ProcessOutboxMessagesJob";

        public int ScheduleIntervalInSeconds { get; set; }
    }
}