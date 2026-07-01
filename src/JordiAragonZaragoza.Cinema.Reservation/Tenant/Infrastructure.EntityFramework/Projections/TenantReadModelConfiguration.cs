namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class TenantReadModelConfiguration : BaseReadModelTypeConfiguration<TenantReadModel, Guid, ReservationReadModelContext>
    {
        public TenantReadModelConfiguration(ReservationReadModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}