namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.CommandHandlers.CreateTenant
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class CreateTenantCommandHandler : ICommandHandler<CreateTenantCommand>
    {
        private readonly IRepository<Tenant, TenantId> tenantRepository;

        public CreateTenantCommandHandler(IRepository<Tenant, TenantId> tenantRepository)
        {
            this.tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
        }

        public async Task<Result> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var newTenant = Tenant.Create(
                id: new TenantId(request.TenantId));

            await this.tenantRepository.AddAsync(newTenant, cancellationToken);

            return Result.NoContent();
        }
    }
}