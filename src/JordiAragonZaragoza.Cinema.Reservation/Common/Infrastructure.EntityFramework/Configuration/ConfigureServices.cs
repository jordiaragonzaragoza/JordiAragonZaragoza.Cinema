namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration
{
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.ReadModel;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureEntityFrameworkDbReadModel(this IServiceCollection serviceCollection, IConfiguration configuration, bool isDevelopment)
        {
            serviceCollection.AddDbContext<ReservationReadModelContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.JordiAragonZaragozaCinemaReservationReadModelStore))
                                  .ConfigureWarnings(w => w.Ignore(CoreEventId.DuplicateDependentEntityTypeInstanceWarning));

                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            serviceCollection.AddDatabaseDeveloperPageExceptionFilter();

            serviceCollection.AddReadModelsRepositories();

            return serviceCollection;
        }

        public static IHostApplicationBuilder AddInfrastructureEntityFrameworkDbReadModel(this IHostApplicationBuilder hostApplicationBuilder)
        {
            // Configures retries, health check, logging and telemetry for the DbContext.
            hostApplicationBuilder.EnrichNpgsqlDbContext<ReservationReadModelContext>(
                configureSettings: settings =>
                {
                    settings.CommandTimeout = 30;
                });

            return hostApplicationBuilder;
        }

        private static IServiceCollection AddReadModelsRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<,>), typeof(ReservationReadModelRepository<>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(ReservationReadModelRepository<>));
            services.AddScoped(typeof(IReadListRepository<,>), typeof(ReservationReadModelRepository<>));
            services.AddScoped(typeof(ISpecificationReadRepository<,>), typeof(ReservationReadModelRepository<>));
            services.AddScoped(typeof(IPaginatedSpecificationReadRepository<>), typeof(ReservationReadModelRepository<>));
            services.AddScoped(typeof(IRangeableRepository<,>), typeof(ReservationReadModelRepository<>));

            return services;
        }
    }
}