namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class PartitionRemovedEvent(
        Guid AggregateId)
        : BaseDomainEvent(AggregateId);
}