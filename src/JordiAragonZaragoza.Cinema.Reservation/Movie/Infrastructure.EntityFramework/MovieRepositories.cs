namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EntityFramework
{
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class MovieRepositories
    {
        public static IServiceCollection AddMovieBusinessModelRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Movie, MovieId>, ReservationRepository<Movie, MovieId>>();
            services.AddScoped<IReadRepository<Movie, MovieId>, ReservationRepository<Movie, MovieId>>();
            services.AddScoped<IReadListRepository<Movie, MovieId>, ReservationRepository<Movie, MovieId>>();
            services.AddScoped<ISpecificationReadRepository<Movie, MovieId>, ReservationRepository<Movie, MovieId>>();

            return services;
        }
    }
}