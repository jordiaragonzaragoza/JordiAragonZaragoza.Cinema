namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain
{
    using System.Collections.Generic;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;

    public sealed class Assignment : BaseEntity<ScopeId>
    {
        private readonly HashSet<Role> roles = new();

        private readonly HashSet<Permission> permissions = new();

        internal Assignment(ScopeId id)
            : base(id)
        {
        }

        // Required by EF.
        private Assignment()
        {
        }

        public Scope Scope => this.Id;

        public IReadOnlyCollection<Role> Roles
            => this.roles.AsReadOnly();

        public IReadOnlyCollection<Permission> Permissions
            => this.permissions.AsReadOnly();

        internal void AddRole(Role role)
            => this.roles.Add(role);

        internal void RemoveRole(Role role)
            => this.roles.Remove(role);

        internal void AddPermission(Permission permission)
            => this.permissions.Add(permission);

        internal void RemovePermission(Permission permission)
            => this.permissions.Remove(permission);
    }
}