namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Infrastructure.EventStore
{
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Business;

    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class TenantExtensions
    {
        public static IServiceCollection AddTenantBusinessRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Tenant, TenantId>, ReservationRepository<Tenant, TenantId>>();
            services.AddScoped<IReadRepository<Tenant, TenantId>, ReservationRepository<Tenant, TenantId>>();

            return services;
        }
    }
}