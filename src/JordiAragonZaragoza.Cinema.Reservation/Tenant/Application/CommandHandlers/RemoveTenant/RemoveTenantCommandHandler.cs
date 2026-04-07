namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.CommandHandlers.RemoveTenant
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class RemoveTenantCommandHandler : ICommandHandler<RemoveTenantCommand>
    {
        private readonly IRepository<Tenant, TenantId> tenantRepository;

        public RemoveTenantCommandHandler(IRepository<Tenant, TenantId> tenantRepository)
        {
            this.tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
        }

        public async Task<Result> Handle(RemoveTenantCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var existingTenant = await this.tenantRepository.GetByIdAsync(new TenantId(request.TenantId), cancellationToken);
            if (existingTenant is null)
            {
                return Result.NotFound($"{nameof(Tenant)}: {request.TenantId} not found.");
            }

            // TODO: Before remove Tenant check if there is some scheduled showtime regarding to Tenant via domain service.
            existingTenant.Remove();

            await this.tenantRepository.DeleteAsync(existingTenant, cancellationToken);

            return Result.NoContent();
        }
    }
}