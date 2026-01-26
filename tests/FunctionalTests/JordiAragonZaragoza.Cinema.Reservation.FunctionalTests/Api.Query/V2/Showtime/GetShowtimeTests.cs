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

    public sealed class GetShowtimeTests : BaseHttpRestfulApiFunctionalTests
    {
        public GetShowtimeTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetShowtime_WhenHavingValidArguments_ShouldReturnOneShowtime()
        {
            // Arrange
            Guid showtimeId = SeedData.ExampleShowtime.Id;

            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetShowtime}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(showtimeId), showtimeId.ToString()));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<ShowtimeResponse>(uri.PathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.MovieTitle.Should().Be(SeedData.ExampleMovie.Title);
            response.SessionDateOnUtc.Should().BeCloseTo(SeedData.ExampleShowtime.SessionDateOnUtc, TimeSpan.FromMilliseconds(1));
            response.AuditoriumId.Should().Be(SeedData.ExampleAuditorium.Id);
            response.AuditoriumName.Should().Be(SeedData.ExampleAuditorium.Name);
        }
    }
}