namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.V2.User
{
    using System;
    using System.Net;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts;

    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using Xunit;
    using Xunit.Abstractions;

    using SeedData = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedData;

    public sealed class AssignPermissionTests : BaseHttpRestfulApiFunctionalTests
    {
        public AssignPermissionTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task AssignPermission_WhenHavingValidArguments_ShouldAssignPermissionToUser()
        {
            // Arrange
            var userId = SeedData.ExampleUserToBeAssignedScheduleShowtimePermission.Id;
            var tenantId = SeedData.ExampleTenant.Id;
            var partitionId = SeedData.ExamplePartition.Id;
            var cinemaId = SeedData.ExampleCinema.Id;
            var permission = ShowtimePermisions.ScheduleShowtime;
            string[] initialRoles = { Constants.Role.Admin };
            string[] initialPermissions = [];

            await this.GrantUserAsync(userId, tenantId, partitionId, cinemaId, initialRoles, initialPermissions);

            var request = new AssignPermissionBodyRequest(
                tenantId,
                partitionId,
                cinemaId,
                permission);

            using var content = JsonContent.Create(request);

            var route = $"{Routes.ApiBase}{UserRoutes.AssignPermission}";
            route = route.Replace("{userId}", userId.ToString(), StringComparison.Ordinal);
            var fullUri = new Uri(this.Fixture.HttpClient.BaseAddress!, route);

            // Act
            this.OutputHelper.WriteLine($"Requesting with POST {route}");
            var response = await this.Fixture.HttpClient.PostAsync(fullUri, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        private async Task GrantUserAsync(Guid userId, Guid tenantId, Guid? partitionId, Guid? cinemaId, string[] roles, string[] permissions)
        {
            var request = new GrantUserBodyRequest(tenantId, partitionId, cinemaId, roles, permissions);
            using var content = JsonContent.Create(request);

            var route = $"{Routes.ApiBase}{UserRoutes.GrantUser}";
            route = route.Replace("{userId}", userId.ToString(), StringComparison.Ordinal);
            var fullUri = new Uri(this.Fixture.HttpClient.BaseAddress!, route);

            this.OutputHelper.WriteLine($"Preparing existing assignment with POST {route}");
            var response = await this.Fixture.HttpClient.PostAsync(fullUri, content);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
