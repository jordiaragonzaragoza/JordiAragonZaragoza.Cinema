namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class PartitionReadModelConfiguration : BaseModelTypeConfiguration<PartitionReadModel, Guid>
    {
    }
}