namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;

    public static class MovieRepositories
    {
        public static IServiceCollection AddMovieProjectionsRepositories(this IServiceCollection services)
        {
            // MovieReadModel projection.
            services.AddScoped<IRepository<MovieReadModel, Guid>, ReservationReadModelRepository<MovieReadModel>>();
            services.AddScoped<IReadRepository<MovieReadModel, Guid>, ReservationReadModelRepository<MovieReadModel>>();

            return services;
        }
    }
}