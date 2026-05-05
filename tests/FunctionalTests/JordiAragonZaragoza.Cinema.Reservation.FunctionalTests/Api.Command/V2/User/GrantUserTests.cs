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
    using Xunit;
    using Xunit.Abstractions;

    using SeedData = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedData;

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
            var userId = SeedData.ExampleUser.Id;
            var tenantId = Guid.NewGuid();
            var partitionId = Guid.NewGuid();
            var cinemaId = Guid.NewGuid();
            var roles = new[] { "Admin", "Viewer" };

            var request = new GrantUserBodyRequest(
                tenantId,
                partitionId,
                cinemaId,
                roles);

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
