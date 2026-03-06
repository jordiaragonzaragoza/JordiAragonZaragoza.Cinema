namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure
{
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure.Api.Command;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddCommandService();

            return services;
        }

        private static IServiceCollection AddCommandService(this IServiceCollection services)
        {
            services.AddOptions<CommandServiceOptions>()
                .BindConfiguration(CommandServiceOptions.Section)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            ////services.AddTransient<AuthorizationDelegatingHandler>();

            services.AddHttpClient<CommandService>(
                (serviceProvider, httpClient) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<CommandServiceOptions>>().Value;
                    httpClient.BaseAddress = options.Url;
                });
            ////.AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            return services;
        }
    }
}