namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.ReadModels;

    public sealed class UserAuthorizationReadModel : BaseReadModel
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

        public Guid UserId { get; set; }

        public Guid TenantId { get; set; }

        public Guid? PartitionId { get; set; }

        public Guid? DomainId { get; set; }

        /// <summary>
        /// Gets the collection of roles assigned at this scope level.
        /// Uses owned entities pattern for flexibility and database portability.
        /// </summary>
        public ICollection<RoleReadModel> Roles { get; private set; } = new List<RoleReadModel>();

        /// <summary> Gets the collection of permissions assigned at this scope level.
        /// Uses owned entities pattern for flexibility and database portability.
        /// </summary>
        public ICollection<PermissionReadModel> Permissions { get; private set; } = new List<PermissionReadModel>();

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
            return this.TenantId == tenantId
                && (this.PartitionId is null || this.PartitionId == partitionId)
                && (this.DomainId is null || this.DomainId == domainId);
        }
    }
}