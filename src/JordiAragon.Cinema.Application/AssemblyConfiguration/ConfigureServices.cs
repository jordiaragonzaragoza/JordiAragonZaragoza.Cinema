namespace JordiAragon.Cinema.Application.AssemblyConfiguration
{
    using JordiAragon.Cinema.Application.Features.Auditorium.Ticket.BackgroundJobs;
    using JordiAragon.SharedKernel.Application.Behaviours;
    using MediatR;
    using MediatR.NotificationPublishers;
    using MediatR.Pipeline;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Quartz;

    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddAutoMapper(ApplicationAssemblyReference.Assembly);

            serviceCollection.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(ApplicationAssemblyReference.Assembly);
                configuration.NotificationPublisher = new TaskWhenAllPublisher();
            });

            // Register pipeline behaviors for validations and other stuff.
            serviceCollection.AddTransient(typeof(IRequestPreProcessor<>), typeof(LoggerBehaviour<>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(InvalidateCachingBehavior<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(DomainEventsDispatcherBehaviour<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

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

                configure.UseMicrosoftDependencyInjectionJobFactory();
            });

            return serviceCollection;
        }
    }
}