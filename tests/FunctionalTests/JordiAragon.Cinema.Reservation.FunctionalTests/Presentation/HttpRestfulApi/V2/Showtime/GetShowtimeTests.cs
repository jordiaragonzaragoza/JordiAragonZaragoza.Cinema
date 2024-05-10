namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.Showtime
{
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2;
    using Xunit;
    using Xunit.Abstractions;

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
            var showtimeId = SeedData.ExampleShowtime.Id;

            var route = $"api/v2/{GetShowtime.Route}";
            string pathAndQuery = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(showtimeId), showtimeId.ToString()));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<ShowtimeResponse>(pathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.MovieTitle.Should().Be(SeedData.ExampleMovie.Title);
            response.SessionDateOnUtc.Should().Be(SeedData.ExampleShowtime.SessionDateOnUtc);
            response.AuditoriumId.Should().Be(SeedData.ExampleAuditorium.Id);
            response.AuditoriumName.Should().Be(SeedData.ExampleAuditorium.Name);
        }
    }
}