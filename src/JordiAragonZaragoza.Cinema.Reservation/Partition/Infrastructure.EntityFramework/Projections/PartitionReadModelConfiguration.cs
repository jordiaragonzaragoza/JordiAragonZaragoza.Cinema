namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class PartitionReadModelConfiguration : BaseReadModelTypeConfiguration<PartitionReadModel, Guid, ReservationReadModelContext>
    {
        public PartitionReadModelConfiguration(ReservationReadModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}