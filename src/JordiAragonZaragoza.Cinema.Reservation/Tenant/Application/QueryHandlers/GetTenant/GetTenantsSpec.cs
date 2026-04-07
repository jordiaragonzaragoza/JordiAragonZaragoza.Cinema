namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.QueryHandlers.GetTenants
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetTenantsSpec : Specification<TenantReadModel>, IPaginatedSpecification<TenantReadModel>
    {
        private readonly GetTenantsQuery request;

        public GetTenantsSpec(GetTenantsQuery request)
        {
            this.request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public IPaginatedQuery Request
            => this.request;
    }
}