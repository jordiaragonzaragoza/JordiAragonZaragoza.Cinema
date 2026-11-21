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

    using SeedValues = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedValues;

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
            Guid reservationId = SeedValues.ExampleReservationId;

            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetShowtimeReservation}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(reservationId), reservationId.ToString()));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<ReservationResponse>(uri.PathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(SeedValues.ExampleReservationId);
            response.UserId.Should().Be(SeedValues.ExampleUserWithReservationId);
            response.ShowtimeId.Should().Be(SeedValues.ExampleShowtimeId);
            response.SessionDateOnUtc.Should().BeCloseTo(this.Fixture.SeedData.ExampleShowtime.SessionDateOnUtc, TimeSpan.FromMilliseconds(1));
            response.AuditoriumName.Should().Be(this.Fixture.SeedData.ExampleAuditorium.Name);
            response.MovieTitle.Should().Be(this.Fixture.SeedData.ExampleMovie.Title);
        }
    }
}