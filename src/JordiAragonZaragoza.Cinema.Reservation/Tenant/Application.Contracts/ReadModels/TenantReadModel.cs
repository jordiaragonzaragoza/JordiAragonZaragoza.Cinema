namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.ReadModels;

    public sealed class TenantReadModel : BaseReadModel
    {
        public TenantReadModel(
            Guid id)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
        }

        // Required by EF.
        private TenantReadModel()
        {
        }
    }
}