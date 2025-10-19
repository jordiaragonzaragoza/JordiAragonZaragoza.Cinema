namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class ConfigureServices
    {
        public static IServiceCollection AddEntityFrameworkServices(this IServiceCollection serviceCollection, IConfiguration configuration, bool isDevelopment)
        {
            serviceCollection.AddDbContext<ReservationBusinessModelContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.JordiAragonZaragozaCinemaReservationBusinessModelStore))
                                  .ConfigureWarnings(w => w.Ignore(CoreEventId.DuplicateDependentEntityTypeInstanceWarning));
            });

            serviceCollection.AddDbContext<ReservationReadModelContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.JordiAragonZaragozaCinemaReservationReadModelStore))
                                  .ConfigureWarnings(w => w.Ignore(CoreEventId.DuplicateDependentEntityTypeInstanceWarning));

                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            ////serviceCollection.AddHealthChecks().AddDbContextCheck<ReservationBusinessModelContext>();

            ////serviceCollection.AddHealthChecks().AddDbContextCheck<ReservationReadModelContext>();

            serviceCollection.AddDatabaseDeveloperPageExceptionFilter();

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