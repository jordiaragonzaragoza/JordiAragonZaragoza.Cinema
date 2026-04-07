namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class TenantReadModel : IReadModel
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

        public Guid Id { get; private set; }
    }
}