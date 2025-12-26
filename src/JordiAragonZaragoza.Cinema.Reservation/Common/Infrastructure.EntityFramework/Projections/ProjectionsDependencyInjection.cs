namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections
{
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class ProjectionsDependencyInjection
    {
        public static IServiceCollection AddInfrastructureEntityFrameworkProjections(this IServiceCollection serviceCollection, IConfiguration configuration, bool isDevelopment)
        {
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

            return services;
        }
    }
}