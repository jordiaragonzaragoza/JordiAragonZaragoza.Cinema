namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Rules;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class User : BaseEventSourcedAggregateRoot<UserId, Guid>
    {
        private readonly List<Assignment> assignments = new();

        // Required by EF.
        private User()
        {
        }

        public IReadOnlyCollection<Assignment> Assignments => this.assignments.AsReadOnly();

        public static User Create(
            UserId id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            var user = new User();

            user.Apply(new UserCreatedEvent(id));

            return user;
        }

        public void Remove()
            => this.Apply(new UserRemovedEvent(this.Id));

        public IEnumerable<Role> GetRolesFor(
            TenantId tenantId,
            PartitionId? partitionId = default,
            CinemaId? cinemaId = default)
        {
            ArgumentNullException.ThrowIfNull(tenantId, nameof(tenantId));

            return this.assignments
                .Where(a => a.Scope.Matches(cinemaId, partitionId, tenantId))
                .SelectMany(a => a.Roles)
                .Distinct();
        }

        public IEnumerable<Permission> GetPermissionsFor(
        TenantId tenantId,
        PartitionId? partitionId = default,
        CinemaId? cinemaId = default)
        {
            ArgumentNullException.ThrowIfNull(tenantId, nameof(tenantId));

            return this.assignments
                .Where(a => a.Scope.Matches(cinemaId, partitionId, tenantId))
                .SelectMany(a => a.Permissions)
                .Distinct();
        }

        public void GrantUser(
            Scope scope,
            IEnumerable<Role>? roles,
            IEnumerable<Permission>? permissions)
        {
            ArgumentNullException.ThrowIfNull(scope, nameof(scope));

            CheckRule(new AssignmentMustHaveAtLeastOneRoleOrPermissionRule(roles, permissions));

            var assignment = this.assignments
                .FirstOrDefault(a => a.Scope == scope);

            if (assignment is null)
            {
                this.Apply(new UserGrantedEvent(
                    this.Id,
                    scope.TenantId,
                    scope.PartitionId!,
                    scope.CinemaId!,
                    roles?.Select(x => x.Value) ?? Enumerable.Empty<string>(),
                    permissions?.Select(x => x.Value) ?? Enumerable.Empty<string>()));

                return;
            }

            CheckRule(new GrantUserMustIntroduceNewRolesOrPermissionsRule(assignment, roles, permissions));

            foreach (var role in roles?.Where(r => !assignment.Roles.Contains(r)) ?? Enumerable.Empty<Role>())
            {
                this.Apply(new RoleAssignedToScopeEvent(this.Id, scope.TenantId, scope.PartitionId!, scope.CinemaId!, role));
            }

            foreach (var permission in permissions?.Where(p => !assignment.Permissions.Contains(p)) ?? Enumerable.Empty<Permission>())
            {
                this.Apply(new PermissionAssignedToScopeEvent(this.Id, scope.TenantId, scope.PartitionId!, scope.CinemaId!, permission));
            }
        }

        public void RevokeUser(Scope scope)
        {
            ArgumentNullException.ThrowIfNull(scope, nameof(scope));

            _ = this.GetRequiredAssignment(scope);

            this.Apply(new UserRevokedEvent(this.Id, scope.TenantId, scope.PartitionId!, scope.CinemaId!));
        }

        public void AssignRole(Scope scope, Role role)
        {
            ArgumentNullException.ThrowIfNull(scope, nameof(scope));
            ArgumentNullException.ThrowIfNull(role, nameof(role));

            var assignment = this.GetRequiredAssignment(scope);
            CheckRule(new OnlyPossibleToAssignRoleOnceRule(assignment, role));

            this.Apply(new RoleAssignedToScopeEvent(this.Id, scope.TenantId, scope.PartitionId!, scope.CinemaId!, role));
        }

        public void RemoveRole(Scope scope, Role role)
        {
            ArgumentNullException.ThrowIfNull(scope, nameof(scope));
            ArgumentNullException.ThrowIfNull(role, nameof(role));

            var assignment = this.GetRequiredAssignment(scope);

            if (!assignment.Roles.Contains(role))
            {
                throw new NotFoundException(nameof(Role), role.Value);
            }

            this.Apply(new RoleRevokedFromScopeEvent(this.Id, scope.TenantId, scope.PartitionId!, scope.CinemaId!, role));
        }

        public void AssignPermission(Scope scope, Permission permission)
        {
            ArgumentNullException.ThrowIfNull(scope, nameof(scope));
            ArgumentNullException.ThrowIfNull(permission, nameof(permission));

            var assignment = this.GetRequiredAssignment(scope);
            CheckRule(new OnlyPossibleToAssignPermissionOnceRule(assignment, permission));

            this.Apply(new PermissionAssignedToScopeEvent(this.Id, scope.TenantId, scope.PartitionId!, scope.CinemaId!, permission));
        }

        public void RemovePermission(Scope scope, Permission permission)
        {
            ArgumentNullException.ThrowIfNull(scope, nameof(scope));
            ArgumentNullException.ThrowIfNull(permission, nameof(permission));

            var assignment = this.GetRequiredAssignment(scope);

            if (!assignment.Permissions.Contains(permission))
            {
                throw new NotFoundException(nameof(Permission), permission.Value);
            }

            this.Apply(new PermissionRevokedFromScopeEvent(this.Id, scope.TenantId, scope.PartitionId!, scope.CinemaId!, permission));
        }

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case UserCreatedEvent @event:
                    this.Applier(@event);
                    break;

                case UserRemovedEvent:
                    break;

                case UserGrantedEvent @event:
                    this.Applier(@event);
                    break;

                case UserRevokedEvent @event:
                    this.Applier(@event);
                    break;

                case RoleAssignedToScopeEvent @event:
                    this.Applier(@event);
                    break;

                case RoleRevokedFromScopeEvent @event:
                    this.Applier(@event);
                    break;

                case PermissionAssignedToScopeEvent @event:
                    this.Applier(@event);
                    break;

                case PermissionRevokedFromScopeEvent @event:
                    this.Applier(@event);
                    break;

                default:
                    throw new EventCannotBeAppliedToAggregateException<User, UserId>(this, domainEvent);
            }
        }

        protected override void EnsureValidState()
        {
            // Not required validation post apply events. This is a deterministic aggregate.
            // All the validations are done on public methods.
        }

        private void Applier(UserCreatedEvent @event)
        {
            this.Id = new UserId(@event.AggregateId);
        }

        private void Applier(UserGrantedEvent @event)
        {
            var scope = Scope.CreateFromGuids(@event.TenantId, @event.PartitionId, @event.CinemaId);

            var assignment = new Assignment(new ScopeId(scope));
            foreach (var role in @event.Roles.Select(r => new Role(r)))
            {
                assignment.AddRole(role);
            }

            foreach (var permission in @event.Permissions.Select(p => new Permission(p)))
            {
                assignment.AddPermission(permission);
            }

            this.assignments.Add(assignment);
        }

        private void Applier(UserRevokedEvent @event)
        {
            var scope = Scope.CreateFromGuids(@event.TenantId, @event.PartitionId, @event.CinemaId);

            this.assignments.RemoveAll(a => a.Scope == scope);
        }

        private void Applier(RoleAssignedToScopeEvent @event)
        {
            var scope = Scope.CreateFromGuids(@event.TenantId, @event.PartitionId, @event.CinemaId);
            var assignment = this.GetRequiredAssignment(scope);

            assignment.AddRole(new Role(@event.Role));
        }

        private void Applier(RoleRevokedFromScopeEvent @event)
        {
            var scope = Scope.CreateFromGuids(@event.TenantId, @event.PartitionId, @event.CinemaId);
            var assignment = this.GetRequiredAssignment(scope);

            assignment.RemoveRole(new Role(@event.Role));
        }

        private void Applier(PermissionAssignedToScopeEvent @event)
        {
            var scope = Scope.CreateFromGuids(@event.TenantId, @event.PartitionId, @event.CinemaId);
            var assignment = this.GetRequiredAssignment(scope);

            assignment.AddPermission(new Permission(@event.Permission));
        }

        private void Applier(PermissionRevokedFromScopeEvent @event)
        {
            var scope = Scope.CreateFromGuids(@event.TenantId, @event.PartitionId, @event.CinemaId);
            var assignment = this.GetRequiredAssignment(scope);

            assignment.RemovePermission(new Permission(@event.Permission));
        }

        private Assignment GetRequiredAssignment(Scope scope)
        {
            var assignment = this.assignments.FirstOrDefault(a => a.Scope == scope);

            if (assignment is null)
            {
                throw new NotFoundException(nameof(Assignment), scope.ToString());
            }

            return assignment;
        }
    }
}