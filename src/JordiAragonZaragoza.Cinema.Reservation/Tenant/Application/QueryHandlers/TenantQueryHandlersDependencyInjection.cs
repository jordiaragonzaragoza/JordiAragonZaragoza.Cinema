namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.QueryHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.QueryHandlers.GetTenants;

    public static class TenantQueryHandlersDependencyInjection
    {
        public static IServiceCollection AddTenantQueryHandlers(this IServiceCollection services)
        {
            services.AddQueryHandler<GetTenantsQuery, PaginatedCollectionOutputDto<TenantReadModel>, GetTenantsQueryHandler>();

            return services;
        }
    }
}