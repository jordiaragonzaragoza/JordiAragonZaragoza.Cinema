namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Projectors.Tenant
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class TenantRemovedEventProjector : BaseEventHandler<TenantRemovedEvent>
    {
        private readonly IRepository<TenantReadModel, Guid> tenantReadModelRepository;

        public TenantRemovedEventProjector(
            IRepository<TenantReadModel, Guid> tenantReadModelRepository)
        {
            this.tenantReadModelRepository = tenantReadModelRepository ?? throw new ArgumentNullException(nameof(tenantReadModelRepository));
        }

        public override async Task HandleAsync(TenantRemovedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var readModel = await this.tenantReadModelRepository.GetByIdAsync(@event.AggregateId, cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(TenantReadModel), @event.AggregateId.ToString());
            }

            await this.tenantReadModelRepository.DeleteAsync(readModel, cancellationToken);
        }
    }
}