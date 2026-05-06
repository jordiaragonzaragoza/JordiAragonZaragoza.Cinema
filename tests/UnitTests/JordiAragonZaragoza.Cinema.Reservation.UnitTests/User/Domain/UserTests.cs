namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;
    using Xunit;

    public class UserTests
    {
        public static IEnumerable<object[]> InvalidArgumentsGrantUser()
        {
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);
            var roles = new List<Role> { Constants.Role.Admin };

            var scopeValues = new object[] { null!, scope };
            var rolesValues = new object[] { null!, new List<Role>(), roles };

            foreach (var scopeValue in scopeValues)
            {
                foreach (var rolesValue in rolesValues)
                {
                    if (scopeValue is null || rolesValue is null || (rolesValue is ICollection<Role> collection && collection.Count == 0))
                    {
                        yield return new object[] { scopeValue!, rolesValue! };
                    }
                }
            }
        }

        public static IEnumerable<object[]> InvalidArgumentsRevokeUser()
        {
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);

            var scopeValues = new object[] { null!, scope };

            foreach (var scopeValue in scopeValues)
            {
                if (scopeValue is null)
                {
                    yield return new object[] { scopeValue! };
                }
            }
        }

        public static IEnumerable<object[]> InvalidArgumentsAssignRole()
        {
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);
            var role = Constants.Role.Admin;

            var scopeValues = new object[] { null!, scope };
            var roleValues = new object[] { null!, role };

            foreach (var scopeValue in scopeValues)
            {
                foreach (var roleValue in roleValues)
                {
                    if (scopeValue is null || roleValue is null)
                    {
                        yield return new object[] { scopeValue!, roleValue! };
                    }
                }
            }
        }

        public static IEnumerable<object[]> InvalidArgumentsRemoveRole()
        {
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);
            var role = Constants.Role.Viewer;

            var scopeValues = new object[] { null!, scope };
            var roleValues = new object[] { null!, role };

            foreach (var scopeValue in scopeValues)
            {
                foreach (var roleValue in roleValues)
                {
                    if (scopeValue is null || roleValue is null)
                    {
                        yield return new object[] { scopeValue!, roleValue! };
                    }
                }
            }
        }

        [Fact]
        public void CreateUser_WhenHavingInCorrectArguments_ShouldThrowArgumentNullException()
        {
            // Arrange
            UserId id = null!;

            // Act
            Func<User> user = () => User.Create(id);

            // Assert
            user.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateUser_WhenHavingCorrectArguments_ShouldCreateUserAndAddUserCreatedEvent()
        {
            // Arrange
            var id = Constants.User.Id;

            // Act
            var user = User.Create(id);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be(id);

            user.Events.Should()
                       .ContainSingle(x => x is UserCreatedEvent)
                       .Which.Should().BeOfType<UserCreatedEvent>()
                       .Which.Should().Match<UserCreatedEvent>(e => e.AggregateId == id);
        }

        [Fact]
        public void RemoveUser_WhenHavingValidArguments_ShouldAddUserRemovedEvent()
        {
            // Arrange.
            var user = CreateUserUtils.Create();

            // Act.
            user.Remove();

            user.Events.Should()
                       .ContainSingle(x => x is UserRemovedEvent)
                       .Which.Should().BeOfType<UserRemovedEvent>()
                       .Which.Should().Match<UserRemovedEvent>(e => e.AggregateId == user.Id);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsGrantUser))]
        public void GrantUser_WhenHavingInvalidArguments_ShouldThrowException(
            Scope scope,
            IEnumerable<Role> roles)
        {
            // Arrange
            var user = CreateUserUtils.Create();

            // Act
            Action grantUser = () => user.GrantUser(scope, roles);

            // Assert
            grantUser.Should().Throw<Exception>();

            user.Events.Should()
                       .NotContain(x => x is UserGrantedEvent);
        }

        [Fact]
        public void GrantUser_WhenNotExistingAssignment_ShouldCreateUserGrantedEvent()
        {
            // Arrange
            var user = CreateUserUtils.Create();
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);
            var roles = new List<Role> { Constants.Role.Admin, Constants.Role.Viewer };

            // Act
            user.GrantUser(scope, roles);

            // Assert
            user.Events.Should()
                       .ContainSingle(x => x is UserGrantedEvent)
                       .Which.Should().BeOfType<UserGrantedEvent>()
                       .Which.Should().Match<UserGrantedEvent>(e =>
                                                                e.AggregateId == user.Id &&
                                                                e.TenantId == scope.TenantId &&
                                                                e.Roles.Count() == roles.Count &&
                                                                e.Roles.All(r => roles.Select(ro => ro.Value).Contains(r)));
        }

        [Fact]
        public void GrantUser_WhenExistingAssignment_ShouldCreateRoleAssignedToScopeEvent()
        {
            // Arrange
            var user = CreateUserUtils.Create();
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);
            var initialRoles = new List<Role> { Constants.Role.Admin };

            // Grant the first role
            user.GrantUser(scope, initialRoles);

            var newRoles = new List<Role> { Constants.Role.Viewer };

            // Act
            user.GrantUser(scope, newRoles);

            // Assert
            user.Events.Should()
                       .ContainSingle(x => x is RoleAssignedToScopeEvent)
                       .Which.Should().BeOfType<RoleAssignedToScopeEvent>()
                       .Which.Should().Match<RoleAssignedToScopeEvent>(e =>
                                                                e.AggregateId == user.Id &&
                                                                e.TenantId == scope.TenantId &&
                                                                e.Role == Constants.Role.Viewer);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsRevokeUser))]
        public void RevokeUser_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            Scope scope)
        {
            // Arrange
            var user = CreateUserUtils.Create();

            // Act
            Action revokeUser = () => user.RevokeUser(scope);

            // Assert
            revokeUser.Should().Throw<ArgumentNullException>();

            user.Events.Should()
                       .NotContain(x => x is UserRevokedEvent);
        }

        [Fact]
        public void RevokeUser_WhenScopeDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var user = CreateUserUtils.Create();
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);

            // Act
            Action revokeUser = () => user.RevokeUser(scope);

            // Assert
            revokeUser.Should().Throw<NotFoundException>();

            user.Events.Should()
                       .NotContain(x => x is UserRevokedEvent);
        }

        [Fact]
        public void RevokeUser_WhenHavingValidArguments_ShouldCreateUserRevokedEvent()
        {
            // Arrange
            var user = CreateUserUtils.Create();
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);
            var roles = new List<Role> { Constants.Role.Admin };

            // Grant a role first
            user.GrantUser(scope, roles);

            // Act
            user.RevokeUser(scope);

            // Assert
            user.Events.Should()
                       .ContainSingle(x => x is UserRevokedEvent)
                       .Which.Should().BeOfType<UserRevokedEvent>()
                       .Which.Should().Match<UserRevokedEvent>(e =>
                                                                e.AggregateId == user.Id &&
                                                                e.TenantId == scope.TenantId);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsAssignRole))]
        public void AssignRole_WhenHavingInvalidArguments_ShouldThrowArgumentNullException(
            Scope scope,
            Role role)
        {
            // Arrange
            var user = CreateUserUtils.Create();

            // Act
            Action assignRole = () => user.AssignRole(scope, role);

            // Assert
            assignRole.Should().Throw<ArgumentNullException>();

            user.Events.Should()
                       .NotContain(x => x is RoleAssignedToScopeEvent);
        }

        [Fact]
        public void AssignRole_WhenScopeDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var user = CreateUserUtils.Create();
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);
            var role = Constants.Role.Viewer;

            // Act
            Action assignRole = () => user.AssignRole(scope, role);

            // Assert
            assignRole.Should().Throw<NotFoundException>();

            user.Events.Should()
                       .NotContain(x => x is RoleAssignedToScopeEvent);
        }

        [Fact]
        public void AssignRole_WhenHavingValidArguments_ShouldCreateRoleAssignedToScopeEvent()
        {
            // Arrange
            var user = CreateUserUtils.Create();
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);
            var initialRoles = new List<Role> { Constants.Role.Admin };

            user.GrantUser(scope, initialRoles);

            // Act
            user.AssignRole(scope, Constants.Role.Viewer);

            // Assert
            user.Events.Should()
                       .ContainSingle(x => x is RoleAssignedToScopeEvent)
                       .Which.Should().BeOfType<RoleAssignedToScopeEvent>()
                       .Which.Should().Match<RoleAssignedToScopeEvent>(e =>
                                                                e.AggregateId == user.Id &&
                                                                e.TenantId == scope.TenantId &&
                                                                e.Role == Constants.Role.Viewer);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsRemoveRole))]
        public void RemoveRole_WhenHavingInvalidArguments_ShouldThrowArgumentNullException(
            Scope scope,
            Role role)
        {
            // Arrange
            var user = CreateUserUtils.Create();

            // Act
            Action removeRole = () => user.RemoveRole(scope, role);

            // Assert
            removeRole.Should().Throw<ArgumentNullException>();

            user.Events.Should()
                       .NotContain(x => x is RoleRevokedFromScopeEvent);
        }

        [Fact]
        public void RemoveRole_WhenScopeDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var user = CreateUserUtils.Create();
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);
            var role = Constants.Role.Viewer;

            // Act
            Action removeRole = () => user.RemoveRole(scope, role);

            // Assert
            removeRole.Should().Throw<NotFoundException>();

            user.Events.Should()
                       .NotContain(x => x is RoleRevokedFromScopeEvent);
        }

        [Fact]
        public void RemoveRole_WhenRoleDoesNotExist_ShouldNotCreateEvent()
        {
            // Arrange
            var user = CreateUserUtils.Create();
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);
            var roles = new List<Role> { Constants.Role.Admin };

            user.GrantUser(scope, roles);

            // Act
            user.RemoveRole(scope, Constants.Role.Viewer);

            // Assert
            user.Events.Should()
                       .NotContain(x => x is RoleRevokedFromScopeEvent);
        }

        [Fact]
        public void RemoveRole_WhenHavingValidArguments_ShouldCreateRoleRevokedFromScopeEvent()
        {
            // Arrange
            var user = CreateUserUtils.Create();
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);
            var roles = new List<Role> { Constants.Role.Admin, Constants.Role.Viewer };

            user.GrantUser(scope, roles);

            // Act
            user.RemoveRole(scope, Constants.Role.Viewer);

            // Assert
            user.Events.Should()
                       .ContainSingle(x => x is RoleRevokedFromScopeEvent)
                       .Which.Should().BeOfType<RoleRevokedFromScopeEvent>()
                       .Which.Should().Match<RoleRevokedFromScopeEvent>(e =>
                                                                e.AggregateId == user.Id &&
                                                                e.TenantId == scope.TenantId &&
                                                                e.Role == Constants.Role.Viewer);
        }

        [Fact]
        public void GetRolesFor_WhenHavingInvalidTenantArgument_ShouldThrowArgumentNullException()
        {
            // Arrange
            var user = CreateUserUtils.Create();
            TenantId tenantId = null!;

            // Act
            Func<List<Role>> getRoles = () => user.GetRolesFor(tenantId).ToList();

            // Assert
            getRoles.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetRolesFor_WhenHavingValidArguments_ShouldReturnRolesForScope()
        {
            // Arrange
            var user = CreateUserUtils.Create();
            var scope = Scope.Create(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);

            user.GrantUser(scope, new List<Role> { Constants.Role.Admin, Constants.Role.Viewer });

            // Act
            var roles = user.GetRolesFor(Constants.Tenant.Id, Constants.Partition.Id, Constants.Cinema.Id);

            // Assert
            roles.Should().HaveCount(2)
                 .And.Contain(Constants.Role.Admin)
                 .And.Contain(Constants.Role.Viewer);
        }
    }
}