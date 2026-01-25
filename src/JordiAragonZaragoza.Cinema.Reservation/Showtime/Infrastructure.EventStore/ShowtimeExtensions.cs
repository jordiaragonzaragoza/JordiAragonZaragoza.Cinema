namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EventStore
{
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Business;

    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class ShowtimeExtensions
    {
        public static IServiceCollection AddShowtimeBusinessRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Showtime, ShowtimeId>, ReservationRepository<Showtime, ShowtimeId>>();
            services.AddScoped<IReadRepository<Showtime, ShowtimeId>, ReservationRepository<Showtime, ShowtimeId>>();

            return services;
        }
    }
}