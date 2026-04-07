namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Projectors.Tenant
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class TenantAddedEventProjector : BaseEventHandler<TenantCreatedEvent>
    {
        private readonly IRepository<TenantReadModel, Guid> tenantReadModelRepository;

        public TenantAddedEventProjector(
            IRepository<TenantReadModel, Guid> tenantReadModelRepository)
        {
            this.tenantReadModelRepository = tenantReadModelRepository ?? throw new ArgumentNullException(nameof(tenantReadModelRepository));
        }

        public override async Task HandleAsync(TenantCreatedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var tenantReadModel = new TenantReadModel(
                @event.AggregateId);

            await this.tenantReadModelRepository.AddAsync(tenantReadModel, cancellationToken);
        }
    }
}