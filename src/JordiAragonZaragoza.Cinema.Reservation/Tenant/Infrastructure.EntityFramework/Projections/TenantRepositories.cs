namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public static class TenantRepositories
    {
        public static IServiceCollection AddTenantProjectionsRepositories(this IServiceCollection services)
        {
            // TenantReadModel projection.
            services.AddScoped<IRepository<TenantReadModel, Guid>, ReservationReadModelRepository<TenantReadModel>>();
            services.AddScoped<IPaginatedSpecificationReadRepository<TenantReadModel>, ReservationReadModelRepository<TenantReadModel>>();

            return services;
        }
    }
}