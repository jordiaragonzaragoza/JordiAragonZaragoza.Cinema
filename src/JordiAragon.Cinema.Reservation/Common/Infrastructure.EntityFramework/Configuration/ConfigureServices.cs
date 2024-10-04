namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.Outbox;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Quartz;
    using Volo.Abp.Guids;

    public static class ConfigureServices
    {
        public static IServiceCollection AddEntityFrameworkServices(this IServiceCollection serviceCollection, IConfiguration configuration, bool isDevelopment)
        {
            serviceCollection.Configure<AbpSequentialGuidGeneratorOptions>(options =>
            {
                // Recomended option to Generate Guids on PostgreSQL Databases.
                options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsString;
            });

            serviceCollection.AddDbContext<ReservationBusinessModelContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("JordiAragonCinemaReservationBusinessModelStore"))
                                  .ConfigureWarnings(w => w.Ignore(CoreEventId.DuplicateDependentEntityTypeInstanceWarning));
            });

            serviceCollection.AddDbContext<ReservationReadModelContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql(configuration.GetConnectionString("JordiAragonCinemaReservationReadModelStore"))
                                  .ConfigureWarnings(w => w.Ignore(CoreEventId.DuplicateDependentEntityTypeInstanceWarning));

                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            ////serviceCollection.AddHealthChecks().AddDbContextCheck<ReservationBusinessModelContext>();

            ////serviceCollection.AddHealthChecks().AddDbContextCheck<ReservationReadModelContext>();

            serviceCollection.AddDatabaseDeveloperPageExceptionFilter();

            serviceCollection.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

                // This Bind is required because AddQuartz dont support IServiceProvider / option pattern.
                var processOutboxMessagesJobOptions = new ProcessOutboxMessagesJobOptions();
                configuration.GetSection(ProcessOutboxMessagesJobOptions.Section).Bind(processOutboxMessagesJobOptions);

                var intervalInSeconds = processOutboxMessagesJobOptions.ScheduleIntervalInSeconds;

                configure.AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                                              .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(intervalInSeconds)
                                                                                      .RepeatForever()));
            });

            return serviceCollection;
        }

        public static IHostApplicationBuilder EnrichDbContexts(this IHostApplicationBuilder hostApplicationBuilder)
        {
            // Configures retries, health check, logging and telemetry for the DbContext".
            hostApplicationBuilder.EnrichNpgsqlDbContext<ReservationBusinessModelContext>();

            hostApplicationBuilder.EnrichNpgsqlDbContext<ReservationReadModelContext>();

            return hostApplicationBuilder;
        }
    }
}