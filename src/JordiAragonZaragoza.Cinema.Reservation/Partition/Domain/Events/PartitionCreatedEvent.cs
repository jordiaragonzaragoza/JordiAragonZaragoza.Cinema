namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Domain.Events
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;

    public sealed record class PartitionCreatedEvent(
        Guid PartitionId)
        : BaseDomainEvent(PartitionId);
}