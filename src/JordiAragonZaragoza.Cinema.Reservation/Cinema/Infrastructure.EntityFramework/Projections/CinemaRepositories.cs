namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public static class CinemaRepositories
    {
        public static IServiceCollection AddCinemaProjectionsRepositories(this IServiceCollection services)
        {
            // CinemaReadModel projection.
            services.AddScoped<IRepository<CinemaReadModel, Guid>, ReservationReadModelRepository<CinemaReadModel>>();
            services.AddScoped<IPaginatedSpecificationReadRepository<CinemaReadModel>, ReservationReadModelRepository<CinemaReadModel>>();

            return services;
        }
    }
}