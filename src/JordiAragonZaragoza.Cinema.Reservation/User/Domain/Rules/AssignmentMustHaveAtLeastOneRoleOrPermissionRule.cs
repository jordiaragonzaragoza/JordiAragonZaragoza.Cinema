namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public class AssignmentMustHaveAtLeastOneRoleOrPermissionRule : IBusinessRule
    {
        public AssignmentMustHaveAtLeastOneRoleOrPermissionRule(
            IEnumerable<Role>? roles,
            IEnumerable<Permission>? permissions)
        {
            this.Roles = roles ?? Enumerable.Empty<Role>();
            this.Permissions = permissions ?? Enumerable.Empty<Permission>();
        }

        public string Message => "An assignment must have at least one role or permission.";

        public IEnumerable<Role> Roles { get; }

        public IEnumerable<Permission> Permissions { get; }

        public bool IsBroken()
        {
            return !this.Roles.Any() && !this.Permissions.Any();
        }
    }
}