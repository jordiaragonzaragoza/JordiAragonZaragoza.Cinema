namespace JordiAragonZaragoza.Cinema.Reservation.Common.Application
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Projectors;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.QueryHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Projectors;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.QueryHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Projectors;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.QueryHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Projectors;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.QueryHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Policies;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Policies.ExpireReservedSeats;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Projectors;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.QueryHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Services;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Quartz;

    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<UserAuthorizationResolver>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IAuthorizationPolicy, ReservationOwnerOrAdminPolicy>();

            return services;
        }

        public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(AssemblyReference.Assembly, ServiceLifetime.Singleton);

            return services;
        }

        public static IServiceCollection AddApplicationCommandHandlers(this IServiceCollection services)
        {
            services.AddAuditoriumCommandHandlers();
            services.AddMovieCommandHandlers();
            services.AddShowtimeCommandHandlers();
            services.AddUserCommandHandlers();
            services.AddCinemaCommandHandlers();
            services.AddPartitionCommandHandlers();
            services.AddTenantCommandHandlers();

            return services;
        }

        public static IServiceCollection AddApplicationQueryHandlers(this IServiceCollection services)
        {
            services.AddAuditoriumQueryHandlers();
            services.AddMovieQueryHandlers();
            services.AddShowtimeQueryHandlers();
            services.AddUserQueryHandlers();
            services.AddCinemaQueryHandlers();
            services.AddPartitionQueryHandlers();
            services.AddTenantQueryHandlers();

            return services;
        }

        public static IServiceCollection AddApplicationProjectors(this IServiceCollection services)
        {
            services.AddShowtimeProjectors();
            services.AddAuditoriumProjectors();
            services.AddMovieProjectors();
            services.AddUserProjectors();
            services.AddCinemaProjectors();
            services.AddPartitionProjectors();
            services.AddTenantProjectors();

            return services;
        }

        public static IServiceCollection AddApplicationPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            // TODO: Replace this batch job to a policy-saga with timeout message.
            services
                .AddQuartz(configure =>
                {
                    var expireReservedSeatsJobKey = new JobKey(nameof(ExpireReservedSeatsJob));

                    // This Bind is required because AddQuartz dont support IServiceProvider / option pattern.
                    // https://github.com/quartznet/quartznet/issues/2607
                    var expireReservedSeatsJobOptions = new ExpireReservedSeatsJobOptions();
                    configuration.GetSection(ExpireReservedSeatsJobOptions.Section).Bind(expireReservedSeatsJobOptions);

                    var expireReservedSeatsIntervalInSeconds = expireReservedSeatsJobOptions.ScheduleIntervalInSeconds;

                    configure
                        .AddJob<ExpireReservedSeatsJob>(expireReservedSeatsJobKey, job => { })
                        .AddTrigger(trigger => trigger.ForJob(expireReservedSeatsJobKey)
                                                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(expireReservedSeatsIntervalInSeconds)
                                                                                            .RepeatForever()));
                })
                .AddQuartzHostedService(opt =>
                {
                    opt.WaitForJobsToComplete = true;
                });

            services.AddShowtimeEventHandlersPolicies();

            return services;
        }
    }
}