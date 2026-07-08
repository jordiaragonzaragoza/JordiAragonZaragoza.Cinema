namespace JordiAragonZaragoza.Cinema.Reservation.ArchitectureTests.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Application;
    using JordiAragonZaragoza.SharedKernel.Application.Attributes;
    using Xunit;

    public sealed class AuthorizationTests
    {
        // Verifies consistency between ALL constant classes of permissions/roles
        // in Application.Contracts (of any bounded context) and the strings used in [Authorize]
        [Fact]
        public void AllAuthorizeRolesAndPermissionsMustExistInKnownConstants()
        {
            var knownRoles = CollectConstantsFrom(typeof(Roles));

            var knownPermissions = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName!.StartsWith("JordiAragonZaragoza.Cinema.Reservation", StringComparison.InvariantCulture))
                .SelectMany(a => a.GetTypes())
                .Where(t => t.Name.EndsWith("Permissions", StringComparison.InvariantCulture) && t.IsAbstract && t.IsSealed) // static classes
                .SelectMany(CollectConstantsFrom)
                .ToHashSet();

            var declaredInAttributes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .SelectMany(t => t.GetCustomAttributes<AuthorizeAttribute>())
                .ToList();

            foreach (var attr in declaredInAttributes)
            {
                foreach (var role in attr.Roles?.Split(',') ?? [])
                {
                    knownRoles.Should().Contain(
                        role.Trim(),
                        $"role '{role}' used in [Authorize] is not declared in Roles constants");
                }

                foreach (var permission in attr.Permissions?.Split(',') ?? [])
                {
                    knownPermissions.Should().Contain(
                        permission.Trim(),
                        $"permission '{permission}' used in [Authorize] is not declared in any *Permissions class");
                }
            }
        }

        private static HashSet<string> CollectConstantsFrom(Type type)
            => type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.IsLiteral)
                .Select(f => (string)f.GetRawConstantValue()!)
                .ToHashSet();
    }
}