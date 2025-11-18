namespace JordiAragonZaragoza.Cinema.Reservation.Common.Domain
{
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using Microsoft.Extensions.DependencyInjection;

    public static class DomainDependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddShowtimeDomainServices();

            return services;
        }
    }
}