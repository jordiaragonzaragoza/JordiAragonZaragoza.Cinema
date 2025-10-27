namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EventStore
{
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class MovieExtensions
    {
        public static IServiceCollection AddMovie(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Movie, MovieId>, ReservationRepository<Movie, MovieId>>();
            services.AddScoped<IReadRepository<Movie, MovieId>, ReservationRepository<Movie, MovieId>>();

            return services;
        }
    }
}