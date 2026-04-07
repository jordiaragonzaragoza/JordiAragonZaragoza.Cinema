namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class PartitionReadModel : IReadModel
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

        public Guid Id { get; private set; }
    }
}