namespace JordiAragonZaragoza.Cinema.Reservation.Worker.ReadModelMigrator.Configuration
{
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Interceptors;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.Context;

    public static class ReadModelMigratorDependencyInjection
    {
        public static IServiceCollection AddInfrastructureEntityFrameworkMigrations(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton<TenantReadModelSaveChangesInterceptor>();
            serviceCollection.AddSingleton<IExecutionContextService, ExecutionContextService>();

            serviceCollection.AddDbContext<ReservationReadModelContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql(
                                    configuration.GetConnectionString(Constants.ReservationReadModelStore))
                                    .ConfigureWarnings(w => w.Ignore(CoreEventId.DuplicateDependentEntityTypeInstanceWarning));
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