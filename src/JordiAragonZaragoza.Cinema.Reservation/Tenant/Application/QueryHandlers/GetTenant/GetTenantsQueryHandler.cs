namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.QueryHandlers.GetTenants
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    // TODO: Temporal. Move. This query is part of other bounded context(Catalog)
    public sealed class GetTenantsQueryHandler : IQueryHandler<GetTenantsQuery, PaginatedCollectionOutputDto<TenantReadModel>>
    {
        private readonly IPaginatedSpecificationReadRepository<TenantReadModel> auditoriumReadModelRepository;

        public GetTenantsQueryHandler(IPaginatedSpecificationReadRepository<TenantReadModel> auditoriumReadModelRepository)
        {
            this.auditoriumReadModelRepository = auditoriumReadModelRepository ?? throw new ArgumentNullException(nameof(auditoriumReadModelRepository));
        }

        public async Task<Result<PaginatedCollectionOutputDto<TenantReadModel>>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetTenantsSpec(request);
            var result = await this.auditoriumReadModelRepository.PaginatedListAsync(specification, cancellationToken);
            if (!result.Items.Any())
            {
                return Result.NotFound("Tenant/s not found.");
            }

            return Result.Success(result);
        }
    }
}