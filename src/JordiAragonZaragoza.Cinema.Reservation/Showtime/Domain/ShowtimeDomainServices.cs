namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ShowtimeDomainServices
    {
        public static IServiceCollection AddShowtimeDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IReservationManager, ReservationManager>();

            return services;
        }
    }
}