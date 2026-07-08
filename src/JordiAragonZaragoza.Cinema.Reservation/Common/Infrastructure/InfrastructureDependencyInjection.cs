namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure
{
    using JordiAragonZaragoza.Cinema.ServiceDefaults;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using Microsoft.Extensions.Hosting;

    public static class InfrastructureDependencyInjection
    {
        public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
        {
            builder.AddServiceDefaults();
            builder.AddRedisDistributedCache(Constants.RedisCache);

            return builder;
        }
    }
}