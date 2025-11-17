namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;

    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class AuditoriumRepositories
    {
        public static IServiceCollection AddAuditoriumProjectionsRepositories(this IServiceCollection services)
        {
            // AuditoriumReadModel projection.
            services.AddScoped<IRepository<AuditoriumReadModel, Guid>, ReservationReadModelRepository<AuditoriumReadModel>>();
            services.AddScoped<IReadRepository<AuditoriumReadModel, Guid>, ReservationReadModelRepository<AuditoriumReadModel>>();

            return services;
        }
    }
}