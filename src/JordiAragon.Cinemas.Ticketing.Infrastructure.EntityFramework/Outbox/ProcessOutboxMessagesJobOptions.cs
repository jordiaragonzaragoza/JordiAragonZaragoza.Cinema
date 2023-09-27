namespace JordiAragon.Cinemas.Ticketing.Infrastructure.EntityFramework.Outbox
{
    using System;

    public class ProcessOutboxMessagesJobOptions
    {
        public const string Section = "BackgroundJobs:ProcessOutboxMessagesJob";

        public int ScheduleIntervalInSeconds { get; set; }
    }
}