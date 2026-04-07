namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class TenantReadModelConfiguration : BaseModelTypeConfiguration<TenantReadModel, Guid>
    {
    }
}