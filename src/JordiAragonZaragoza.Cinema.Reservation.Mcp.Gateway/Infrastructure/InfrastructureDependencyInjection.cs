namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure
{
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure.Api.Command.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure.Api.Query.V2;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using Microsoft.Extensions.DependencyInjection;

    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddCommandService();
            services.AddQueryService();

            return services;
        }

        private static IServiceCollection AddCommandService(this IServiceCollection services)
        {
            ////services.AddTransient<AuthorizationDelegatingHandler>();
            services.AddHttpClient<ApiCommandClient>(
                static client => client.BaseAddress = new($"https+http://{Constants.ReservationApiCommand}"));
            ////.AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            return services;
        }

        private static IServiceCollection AddQueryService(this IServiceCollection services)
        {
            ////services.AddTransient<AuthorizationDelegatingHandler>();
            services.AddHttpClient<ApiQueryClient>(
                static client => client.BaseAddress = new($"https+http://{Constants.ReservationApiQuery}"));
            ////.AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            return services;
        }
    }
}