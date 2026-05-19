namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public class GrantUserMustIntroduceNewRolesOrPermissionsRule : IBusinessRule
    {
        private readonly Assignment existingAssignment;
        private readonly IEnumerable<Role> requestedRoles;
        private readonly IEnumerable<Permission> requestedPermissions;

        public GrantUserMustIntroduceNewRolesOrPermissionsRule(
            Assignment existingAssignment,
            IEnumerable<Role>? requestedRoles,
            IEnumerable<Permission>? requestedPermissions)
        {
            this.existingAssignment = existingAssignment;
            this.requestedRoles = requestedRoles ?? Enumerable.Empty<Role>();
            this.requestedPermissions = requestedPermissions ?? Enumerable.Empty<Permission>();
        }

        public string Message => "GrantUser must introduce at least one new role or permission not already assigned to the scope.";

        public bool IsBroken()
        {
            var hasNewRole = this.requestedRoles
                .Any(r => !this.existingAssignment.Roles.Contains(r));

            var hasNewPermission = this.requestedPermissions
                .Any(p => !this.existingAssignment.Permissions.Contains(p));

            return !hasNewRole && !hasNewPermission;
        }
    }
}