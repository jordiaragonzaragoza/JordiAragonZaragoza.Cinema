namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common.Infrastructure
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using Microsoft.Extensions.DependencyInjection;

    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<AuthorizationDelegatingHandler>();

            // These extension methods must return the IHttpClientBuilder of the typed client.
            services.AddReservationCommandClient(new Uri($"https+http://{Constants.ReservationApiCommand}"))
                .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            services.AddReservationQueryClient(new Uri($"https+http://{Constants.ReservationApiQuery}"))
                .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

            return services;
        }
    }
}