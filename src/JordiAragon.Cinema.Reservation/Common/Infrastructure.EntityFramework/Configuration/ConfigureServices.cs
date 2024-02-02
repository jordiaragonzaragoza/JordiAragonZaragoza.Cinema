namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Outbox;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Quartz;
    using Volo.Abp.Guids;

    public static class ConfigureServices
    {
        public static IServiceCollection AddEntityFrameworkServices(this IServiceCollection serviceCollection, IConfiguration configuration, bool isDevelopment)
        {
            serviceCollection.Configure<AbpSequentialGuidGeneratorOptions>(options =>
            {
                // Recomended option to Generate Guids on SQL Server Databases.
                options.DefaultSequentialGuidType = SequentialGuidType.SequentialAtEnd;
            });

            var azureSqlDatabaseOptionsWrite = new AzureSqlDatabaseOptions();
            configuration.Bind(AzureSqlDatabaseOptions.WriteSection, azureSqlDatabaseOptionsWrite);
            serviceCollection.AddSingleton(Options.Create(azureSqlDatabaseOptionsWrite));

            serviceCollection.AddDbContext<ReservationWriteContext>(optionsBuilder =>
            {
                if (isDevelopment)
                {
                    /*optionsBuilder.UseInMemoryDatabase("JordiAragon.Cinema.Reservation.WriteStore")
                                  .EnableSensitiveDataLogging()
                                  .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));*/

                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("WriteStore"))
                                  .ConfigureWarnings(w => w.Ignore(CoreEventId.DuplicateDependentEntityTypeInstanceWarning));
                }
                else
                {
                    optionsBuilder.UseSqlServer(azureSqlDatabaseOptionsWrite.BuildConnectionString());
                }
            });

            var azureSqlDatabaseOptionsRead = new AzureSqlDatabaseOptions();
            configuration.Bind(AzureSqlDatabaseOptions.ReadSection, azureSqlDatabaseOptionsRead);
            serviceCollection.AddSingleton(Options.Create(azureSqlDatabaseOptionsRead));

            serviceCollection.AddDbContext<ReservationReadContext>(optionsBuilder =>
            {
                if (isDevelopment)
                {
                    /*optionsBuilder.UseInMemoryDatabase("JordiAragon.Cinema.Reservation.ReadStore")
                                  .EnableSensitiveDataLogging()
                                  .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));*/

                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("ReadStore"))
                                  .ConfigureWarnings(w => w.Ignore(CoreEventId.DuplicateDependentEntityTypeInstanceWarning));
                }
                else
                {
                    optionsBuilder.UseSqlServer(azureSqlDatabaseOptionsRead.BuildConnectionString());
                }

                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            serviceCollection.AddHealthChecks().AddDbContextCheck<ReservationWriteContext>();

            serviceCollection.AddHealthChecks().AddDbContextCheck<ReservationReadContext>();

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
    }
}