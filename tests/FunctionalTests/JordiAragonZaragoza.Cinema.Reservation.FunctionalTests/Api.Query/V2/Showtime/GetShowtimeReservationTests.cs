namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.V2.Showtime
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.Common;
    using Xunit;
    using Xunit.Abstractions;

    using SeedData = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedData;

    public sealed class GetShowtimeReservationTests : BaseHttpRestfulApiFunctionalTests
    {
        public GetShowtimeReservationTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetShowtime_WhenHavingValidArguments_ShouldReturnOneShowtime()
        {
            // Arrange
            Guid reservationId = SeedData.ExampleReservation.Id;

            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetShowtimeReservation}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(reservationId), reservationId.ToString()));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<ReservationResponse>(uri.PathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(SeedData.ExampleReservation.Id);
            response.UserId.Should().Be(SeedData.ExampleUser.Id);
            response.ShowtimeId.Should().Be(SeedData.ExampleShowtime.Id);
            response.SessionDateOnUtc.Should().BeCloseTo(SeedData.ExampleShowtime.SessionDateOnUtc, TimeSpan.FromMilliseconds(1));
            response.AuditoriumName.Should().Be(SeedData.ExampleAuditorium.Name);
            response.MovieTitle.Should().Be(SeedData.ExampleMovie.Title);
        }
    }
}