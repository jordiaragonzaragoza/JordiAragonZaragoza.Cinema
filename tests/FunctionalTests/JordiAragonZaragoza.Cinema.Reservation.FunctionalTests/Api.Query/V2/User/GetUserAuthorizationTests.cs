namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.V2.User
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Responses;

    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.Common;
    using Xunit;
    using Xunit.Abstractions;

    using SeedData = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedData;

    public sealed class GetUserAuthorizationTests : BaseHttpRestfulApiFunctionalTests
    {
        public GetUserAuthorizationTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetUserAuthorization_WhenHavingValidArguments_ShouldReturnUserAuthorization()
        {
            // Arrange
            Guid userId = SeedData.ExampleUserWithAdminRole.Id;
            Guid tenantId = SeedData.ExampleTenant.Id;
            Guid partitionId = SeedData.ExamplePartition.Id;
            Guid cinemaId = SeedData.ExampleCinema.Id;

            var route = $"{Routes.ApiBase}{UserRoutes.GetUserAuthorization}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(userId), userId.ToString()),
                (nameof(tenantId), tenantId.ToString()),
                (nameof(partitionId), partitionId.ToString()),
                (nameof(cinemaId), cinemaId.ToString()));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<UserAuthorizationResponse>(uri.PathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
        }
    }
}