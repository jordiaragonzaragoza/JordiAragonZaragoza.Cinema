namespace JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.Configuration
{
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;

    public static class SeederDependencyInjection
    {
        public static IServiceCollection AddInfrastructureEntityFrameworkMigrations(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<ReservationReadModelContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql(
                                    configuration.GetConnectionString(Constants.ReservationReadModelStore),
                                    options => options.MigrationsAssembly(SeederAssemblyReference.Assembly))
                                    .ConfigureWarnings(w => w.Ignore(CoreEventId.DuplicateDependentEntityTypeInstanceWarning));

                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            return serviceCollection;
        }

        public static IHostApplicationBuilder AddInfrastructureEntityFrameworkMigrations(this IHostApplicationBuilder hostApplicationBuilder)
        {
            // Configures retries, health check, logging and telemetry for the DbContext.
            hostApplicationBuilder.EnrichNpgsqlDbContext<ReservationReadModelContext>();

            return hostApplicationBuilder;
        }
    }
}