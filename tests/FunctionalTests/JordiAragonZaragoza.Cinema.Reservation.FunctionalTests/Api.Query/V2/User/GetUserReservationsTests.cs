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
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.Common;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using Xunit;
    using Xunit.Abstractions;

    using SeedData = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedData;

    public sealed class GetUserReservationsTests : BaseHttpRestfulApiFunctionalTests
    {
        public GetUserReservationsTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAllUserReservations_WhenHavingValidArguments_ShouldReturnOneReservation()
        {
            // Arrange
            Guid userId = SeedData.ExampleUserWithReservation.Id;

            var route = $"{Routes.ApiBase}{UserRoutes.GetUserReservations}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(userId), userId.ToString()!));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<PaginatedCollectionResponse<ReservationResponse>>(uri.PathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.Items.Should().HaveCount(1);
        }
    }
}