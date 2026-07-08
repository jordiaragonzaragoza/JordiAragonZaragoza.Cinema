namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.ReadModels;

    public sealed class PartitionReadModel : BaseReadModel
    {
        public PartitionReadModel(
            Guid id)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
        }

        // Required by EF.
        private PartitionReadModel()
        {
        }
    }
}