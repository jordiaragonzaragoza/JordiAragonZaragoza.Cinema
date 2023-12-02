namespace JordiAragon.Cinema.Reservation.Common.Application
{
    using JordiAragon.Cinema.Reservation.Showtime.Application.BackgroundJobs;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Quartz;

    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddAutoMapper(AssemblyReference.Assembly);

            serviceCollection.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(ExpireReservedSeatsJob));

                // This Bind is required because AddQuartz dont support IServiceProvider / option pattern.
                var prepareCommunicationsJobOptions = new ExpireReservedSeatsJobOptions();
                configuration.GetSection(ExpireReservedSeatsJobOptions.Section).Bind(prepareCommunicationsJobOptions);

                var intervalInSeconds = prepareCommunicationsJobOptions.ScheduleIntervalInSeconds;

                configure.AddJob<ExpireReservedSeatsJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                                              .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(intervalInSeconds)
                                                                                      .RepeatForever()));
            });

            return serviceCollection;
        }
    }
}