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
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using Xunit;
    using Xunit.Abstractions;

    using SeedData = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedData;

    public sealed class AssignRoleTests : BaseHttpRestfulApiFunctionalTests
    {
        public AssignRoleTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task AssignRole_WhenHavingValidArguments_ShouldAssignRoleToUser()
        {
            // Arrange
            var userId = SeedData.ExampleUser.Id;
            var tenantId = SeedData.ExampleTenant.Id;
            var partitionId = SeedData.ExamplePartition.Id;
            var cinemaId = SeedData.ExampleCinema.Id;
            var role = Constants.Role.Viewer;
            string[] initialRoles = { Constants.Role.Admin };

            await this.GrantUserAsync(userId, tenantId, partitionId, cinemaId, initialRoles);

            var request = new AssignRoleBodyRequest(
                tenantId,
                partitionId,
                cinemaId,
                role);

            using var content = JsonContent.Create(request);

            var route = $"{Routes.ApiBase}{UserRoutes.AssignRole}";
            route = route.Replace("{userId}", userId.ToString(), StringComparison.Ordinal);
            var fullUri = new Uri(this.Fixture.HttpClient.BaseAddress!, route);

            // Act
            this.OutputHelper.WriteLine($"Requesting with POST {route}");
            var response = await this.Fixture.HttpClient.PostAsync(fullUri, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        private async Task GrantUserAsync(Guid userId, Guid tenantId, Guid? partitionId, Guid? cinemaId, string[] roles)
        {
            var request = new GrantUserBodyRequest(tenantId, partitionId, cinemaId, roles);
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
