namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.ProjectionCheckpoint;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class ProjectionsDependencyInjection
    {
        public static IServiceCollection AddInfrastructureEntityFrameworkProjections(this IServiceCollection serviceCollection, IConfiguration configuration, bool isDevelopment)
        {
            serviceCollection.AddScoped<IUnitOfWork, ReservationProjectionsStore>();

            serviceCollection.AddDbContext<ReservationReadModelContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.ReservationReadModelStore))
                                  .ConfigureWarnings(w => w.Ignore(CoreEventId.DuplicateDependentEntityTypeInstanceWarning));

                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            serviceCollection.AddDatabaseDeveloperPageExceptionFilter();

            return serviceCollection;
        }

        public static IHostApplicationBuilder AddInfrastructureEntityFrameworkProjections(this IHostApplicationBuilder hostApplicationBuilder)
        {
            // Configures retries, health check, logging and telemetry for the DbContext.
            hostApplicationBuilder.EnrichNpgsqlDbContext<ReservationReadModelContext>(
                configureSettings: settings =>
                {
                    settings.CommandTimeout = 30;
                });

            return hostApplicationBuilder;
        }

        public static IServiceCollection AddInfrastructureProjectionsRepositories(this IServiceCollection services)
        {
            services.AddAuditoriumProjectionsRepositories();
            services.AddMovieProjectionsRepositories();
            services.AddShowtimeProjectionsRepositories();
            services.AddUserProjectionsRepositories();

            _ = services.AddCheckpointProjectionsRepositories();

            return services;
        }

        private static IServiceCollection AddCheckpointProjectionsRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Checkpoint, Guid>, ReservationReadModelRepository<Checkpoint>>();
            services.AddScoped<IReadRepository<Checkpoint, Guid>, ReservationReadModelRepository<Checkpoint>>();

            return services;
        }
    }
}