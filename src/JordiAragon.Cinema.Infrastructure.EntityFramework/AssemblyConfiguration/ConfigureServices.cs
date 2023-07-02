namespace JordiAragon.Cinema.Infrastructure.EntityFramework.AssemblyConfiguration
{
    using JordiAragon.Cinema.Infrastructure.EntityFramework.Outbox;
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

            var azureSqlDatabaseOptions = new AzureSqlDatabaseOptions();
            configuration.Bind(AzureSqlDatabaseOptions.Section, azureSqlDatabaseOptions);
            serviceCollection.AddSingleton(Options.Create(azureSqlDatabaseOptions));

            serviceCollection.AddDbContext<CinemaContext>(optionsBuilder =>
            {
                if (isDevelopment)
                {
                    /*optionsBuilder.UseInMemoryDatabase("JordiAragon.CinemaDb")
                                  .EnableSensitiveDataLogging()
                                  .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));*/

                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("CinemaConnection"));
                }
                else
                {
                    optionsBuilder.UseSqlServer(azureSqlDatabaseOptions.BuildConnectionString());
                }
            });

            serviceCollection.AddHealthChecks().AddDbContextCheck<CinemaContext>();

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

                configure.UseMicrosoftDependencyInjectionJobFactory();
            });

            return serviceCollection;
        }
    }
}