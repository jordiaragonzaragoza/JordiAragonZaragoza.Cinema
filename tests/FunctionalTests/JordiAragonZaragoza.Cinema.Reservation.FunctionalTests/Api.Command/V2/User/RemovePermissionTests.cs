namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.V2.User
{
    using System;
    using System.Net;
    using System.Net.Http;
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

    public sealed class RemovePermissionTests : BaseHttpRestfulApiFunctionalTests
    {
        public RemovePermissionTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task RemovePermission_WhenHavingValidArguments_ShouldRemovePermissionFromUser()
        {
            // Arrange
            var userId = SeedData.ExampleUserToBeRemovedScheduleShowtimePermission.Id;
            var tenantId = SeedData.ExampleTenant.Id;
            var partitionId = SeedData.ExamplePartition.Id;
            var cinemaId = SeedData.ExampleCinema.Id;
            var permission = ShowtimePermisions.ScheduleShowtime;

            string[] initialRoles = { Constants.Role.Admin };
            string[] initialPermissions = { permission };
            await this.GrantUserAsync(userId, tenantId, partitionId, cinemaId, initialRoles, initialPermissions);

            var request = new RemovePermissionBodyRequest(
                tenantId,
                partitionId,
                cinemaId,
                permission);

            using var content = JsonContent.Create(request);

            var route = $"{Routes.ApiBase}{UserRoutes.RemovePermission}";
            route = route.Replace("{userId}", userId.ToString(), StringComparison.Ordinal);
            var fullUri = new Uri(this.Fixture.HttpClient.BaseAddress!, route);

            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, fullUri)
            {
                Content = content,
            };

            // Act
            this.OutputHelper.WriteLine($"Requesting with DELETE {route}");
            var response = await this.Fixture.HttpClient.SendAsync(httpRequestMessage);

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
