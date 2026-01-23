namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure
{
    using JordiAragonZaragoza.Cinema.ServiceDefaults;
    using Microsoft.Extensions.Hosting;

    public static class InfrastructureDependencyInjection
    {
        public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
        {
            builder.AddServiceDefaults();

            return builder;
        }
    }
}