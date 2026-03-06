namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure
{
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure.Api.Command;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using Microsoft.Extensions.DependencyInjection;

    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddCommandService();

            return services;
        }

        private static IServiceCollection AddCommandService(this IServiceCollection services)
        {
            ////services.AddTransient<AuthorizationDelegatingHandler>();
            services.AddHttpClient<CommandService>(
                static client => client.BaseAddress = new($"https+http://{Constants.ReservationApiCommand}"));
            ////.AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            return services;
        }
    }
}