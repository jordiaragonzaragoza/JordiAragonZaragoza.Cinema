namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.V2.Showtime
{
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    public class GetShowtimeTests : BaseWebApiFunctionalTests
    {
        public GetShowtimeTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAllShowtimes_WhenHavingValidArguments_ShouldReturnOneShowtime()
        {
            // Arrange
            var showtimeId = SeedData.ExampleShowtime.Id.ToString();

            var route = $"api/v2/{GetShowtime.Route}";
            string pathAndQuery = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(showtimeId), showtimeId));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<ShowtimeResponse>(pathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.MovieTitle.Should().Be(SeedData.ExampleShowtimeReadModel.MovieTitle);
            response.SessionDateOnUtc.Should().Be(SeedData.ExampleShowtimeReadModel.SessionDateOnUtc);
            response.AuditoriumId.Should().Be(SeedData.ExampleShowtimeReadModel.AuditoriumId);
            response.AuditoriumName.Should().Be(SeedData.ExampleShowtimeReadModel.AuditoriumName);
        }
    }
}