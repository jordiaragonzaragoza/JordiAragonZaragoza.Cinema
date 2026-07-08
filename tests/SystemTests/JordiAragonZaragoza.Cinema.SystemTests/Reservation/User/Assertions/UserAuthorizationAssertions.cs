namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.Assertions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime;
    using Xunit.Abstractions;

    public static class UserAuthorizationAssertions
    {
        public static async Task ShouldContainRolesAsync(
            ReservationQueryTestClient queryClient,
            UserAuthorizationRequest request,
            IEnumerable<string> expectedRoles,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);
            ArgumentNullException.ThrowIfNull(expectedRoles);

            var authorization = await queryClient.GetUserAuthorizationAsync(request, output);

            authorization.Should().NotBeNull();
            authorization!.Roles.Should().Contain(expectedRoles);
        }

        public static async Task ShouldNotContainRoleAsync(
            ReservationQueryTestClient queryClient,
            UserAuthorizationRequest request,
            string role,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);
            ArgumentNullException.ThrowIfNull(role);

            var authorization = await queryClient.GetUserAuthorizationAsync(request, output);

            authorization.Should().NotBeNull();
            authorization!.Roles.Should().NotContain(role);
        }

        public static async Task ShouldNotExistAsync(
            ReservationQueryTestClient queryClient,
            UserAuthorizationRequest request,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);

            var authorization = await queryClient.GetUserAuthorizationAsync(request, output);

            authorization.Should().BeNull();
        }

        public static async Task ShouldContainPermissionsAsync(
            ReservationQueryTestClient queryClient,
            UserAuthorizationRequest request,
            IEnumerable<string> expectedPermissions,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);
            ArgumentNullException.ThrowIfNull(expectedPermissions);

            var authorization = await queryClient.GetUserAuthorizationAsync(request, output);

            authorization.Should().NotBeNull();
            authorization!.Permissions.Should().Contain(expectedPermissions);
        }

        public static async Task ShouldNotContainPermissionAsync(
            ReservationQueryTestClient queryClient,
            UserAuthorizationRequest request,
            string permission,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);
            ArgumentNullException.ThrowIfNull(permission);

            var authorization = await queryClient.GetUserAuthorizationAsync(request, output);

            authorization.Should().NotBeNull();
            authorization!.Permissions.Should().NotContain(permission);
        }
    }
}
