namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class PartitionId : BaseAggregateRootId<Guid>
    {
        public PartitionId(Guid value)
            : base(value)
        {
        }
    }
}