namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.V2.User
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.Common;
    using Xunit;
    using Xunit.Abstractions;

    using SeedData = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedData;

    public sealed class GetUserReservationTests : BaseHttpRestfulApiFunctionalTests
    {
        public GetUserReservationTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetUserReservation_WhenHavingValidArguments_ShouldReturnUserReservation()
        {
            // Arrange
            Guid showtimeId = SeedData.ExampleShowtime.Id;
            Guid reservationId = SeedData.ExampleReservation.Id;
            Guid userId = SeedData.ExampleUser.Id;

            var route = $"{Routes.ApiBase}{UserRoutes.GetUserReservation}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(userId), userId.ToString()!),
                (nameof(showtimeId), showtimeId.ToString()),
                (nameof(reservationId), reservationId.ToString()));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<ReservationResponse>(uri.PathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
        }
    }
}