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

    using SeedValues = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedValues;

    public sealed class GrantUserTests : BaseHttpRestfulApiFunctionalTests
    {
        public GrantUserTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GrantUser_WhenHavingValidArguments_ShouldGrantRolesToUser()
        {
            // Arrange
            var userId = SeedValues.ExampleUserToBeGrantedId;
            var tenantId = SeedValues.ExampleTenantId;
            var partitionId = SeedValues.ExamplePartitionId;
            var cinemaId = SeedValues.ExampleCinemaId;
            string[] roles = { Constants.Role.Admin, Constants.Role.Viewer };
            string[] permissions = [];

            var request = new GrantUserBodyRequest(
                tenantId,
                partitionId,
                cinemaId,
                roles,
                permissions);

            using var content = JsonContent.Create(request);

            var route = $"{Routes.ApiBase}{UserRoutes.GrantUser}";
            route = route.Replace("{userId}", userId.ToString(), StringComparison.Ordinal);
            var fullUri = new Uri(this.Fixture.HttpClient.BaseAddress!, route);

            // Act
            this.OutputHelper.WriteLine($"Requesting with POST {route}");
            var response = await this.Fixture.HttpClient.PostAsync(fullUri, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
