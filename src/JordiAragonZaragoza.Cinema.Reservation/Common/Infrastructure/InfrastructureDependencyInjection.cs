namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure
{
    using JordiAragonZaragoza.Cinema.ServiceDefaults;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services;
        }

        public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
        {
            builder.AddServiceDefaults();

            return builder;
        }
    }
}