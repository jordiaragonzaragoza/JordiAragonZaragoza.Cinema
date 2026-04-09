namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels
{
    using System;
    using System.Collections.Generic;

    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class UserAuthorizationReadModel : IReadModel
    {
        public UserAuthorizationReadModel(
            Guid id)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
        }

        // Required by EF.
        private UserAuthorizationReadModel()
        {
        }

        public Guid Id { get; private set; }

        public Guid UserId { get; set; }

        public Guid TenantId { get; set; }

        public Guid? PartitionId { get; set; }

        public Guid? CinemaId { get; set; }

        /// <summary>
        /// Gets or sets the list of roles assigned at this scope level.
        /// Note: This property has a public setter for EF Core and projection purposes.
        /// In production, it should only be modified through the projection events.
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read-only (read models require setter for EF Core projections)
        public IList<string> Roles { get; set; } = new List<string>();
#pragma warning restore CA2227

        /// <summary>
        /// Checks if this authorization matches the given scope hierarchy.
        /// Cinema-level scope is the most specific, then partition, then tenant.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="partitionId">The partition identifier (optional).</param>
        /// <param name="domainId">The cinema identifier (optional).</param>
        /// <returns>True if this authorization matches the given scope, false otherwise.</returns>
        public bool Matches(Guid tenantId, Guid? partitionId, Guid? domainId)
        {
            if (this.CinemaId is not null)
            {
                return this.CinemaId == domainId;
            }

            if (this.PartitionId is not null)
            {
                return this.PartitionId == partitionId;
            }

            return this.TenantId == tenantId;
        }
    }
}