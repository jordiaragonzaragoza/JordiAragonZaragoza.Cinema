namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework
{
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class ShowtimeRepositories
    {
        public static IServiceCollection AddShowtimeBusinessModelRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Showtime, ShowtimeId>, ReservationRepository<Showtime, ShowtimeId>>();
            services.AddScoped<IReadRepository<Showtime, ShowtimeId>, ReservationRepository<Showtime, ShowtimeId>>();

            return services;
        }
    }
}