namespace JordiAragonZaragoza.Cinema.Reservation.Worker.Projector.Configuration
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    public static class ConfigureHostBuilder
    {
        public static IServiceCollection AddHostConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSerilog(lc => lc
                .ReadFrom.Configuration(configuration));

            serviceCollection.AddOptions<ServiceProviderOptions>()
                .Configure(options => options.ValidateOnBuild = true);

            return serviceCollection;
        }
    }
}