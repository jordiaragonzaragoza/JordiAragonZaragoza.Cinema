namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.Queries
{
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;

    public sealed record class GetTenantsQuery(
        int PageNumber,
        int PageSize)
            : IPaginatedQuery, IQuery<PaginatedCollectionOutputDto<TenantReadModel>>;
}