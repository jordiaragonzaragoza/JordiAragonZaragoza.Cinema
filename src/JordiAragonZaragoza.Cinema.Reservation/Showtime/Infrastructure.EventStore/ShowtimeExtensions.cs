namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EventStore
{
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class ShowtimeExtensions
    {
        public static IServiceCollection AddShowtime(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Showtime, ShowtimeId>, ReservationRepository<Showtime, ShowtimeId>>();
            services.AddScoped<IReadRepository<Showtime, ShowtimeId>, ReservationRepository<Showtime, ShowtimeId>>();

            return services;
        }
    }
}