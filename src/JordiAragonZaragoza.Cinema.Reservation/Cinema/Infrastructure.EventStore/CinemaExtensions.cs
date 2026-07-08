namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Infrastructure.EventStore
{
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Business;

    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class CinemaExtensions
    {
        public static IServiceCollection AddCinemaBusinessRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Cinema, CinemaId>, ReservationRepository<Cinema, CinemaId>>();
            services.AddScoped<IReadRepository<Cinema, CinemaId>, ReservationRepository<Cinema, CinemaId>>();

            return services;
        }
    }
}