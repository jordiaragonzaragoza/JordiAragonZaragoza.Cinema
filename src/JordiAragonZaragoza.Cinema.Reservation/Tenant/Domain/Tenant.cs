namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;

    public sealed class Tenant : BaseEventSourcedAggregateRoot<TenantId, Guid>
    {
        // Required by EF.
        private Tenant()
        {
        }

        public static Tenant Create(
            TenantId id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            var tenant = new Tenant();

            tenant.Apply(new TenantCreatedEvent(id));

            return tenant;
        }

        public void Remove()
            => this.Apply(new TenantRemovedEvent(this.Id));

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case TenantCreatedEvent @event:
                    this.Applier(@event);
                    break;

                case TenantRemovedEvent:
                    break;

                default:
                    throw new EventCannotBeAppliedToAggregateException<Tenant, TenantId>(this, domainEvent);
            }
        }

        protected override void EnsureValidState()
        {
            // Not required validation post apply events. This is a deterministic aggregate.
            // All the validations are done on public methods.
        }

        private void Applier(TenantCreatedEvent @event)
        {
            this.Id = new TenantId(@event.AggregateId);
        }
    }
}