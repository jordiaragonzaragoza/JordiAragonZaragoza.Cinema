namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Domain
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;

    public sealed class Partition : BaseEventSourcedAggregateRoot<PartitionId, Guid>
    {
        // Required by EF.
        private Partition()
        {
        }

        public static Partition Create(
            PartitionId id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            var partition = new Partition();

            partition.Apply(new PartitionCreatedEvent(id));

            return partition;
        }

        public void Remove()
            => this.Apply(new PartitionRemovedEvent(this.Id));

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case PartitionCreatedEvent @event:
                    this.Applier(@event);
                    break;

                case PartitionRemovedEvent:
                    break;

                default:
                    throw new EventCannotBeAppliedToAggregateException<Partition, PartitionId>(this, domainEvent);
            }
        }

        protected override void EnsureValidState()
        {
            // Not required validation post apply events. This is a deterministic aggregate.
            // All the validations are done on public methods.
        }

        private void Applier(PartitionCreatedEvent @event)
        {
            this.Id = new PartitionId(@event.AggregateId);
        }
    }
}