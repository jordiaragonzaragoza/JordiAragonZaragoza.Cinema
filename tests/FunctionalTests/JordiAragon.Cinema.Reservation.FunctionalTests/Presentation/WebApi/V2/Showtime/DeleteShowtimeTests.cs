namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.V2.Showtime
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.Common;
    using JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    public class DeleteShowtimeTests : BaseWebApiFunctionalTests
    {
        public DeleteShowtimeTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task DeleteShowtime_WhenHavingValidArguments_ShouldDeleteRequiredShowtime()
        {
            // Arrange
            var showtimeId = SeedData.ExampleShowtime.Id.ToString();

            var route = $"api/v2/{DeleteShowtime.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString());

            // Act
            this.OutputHelper.WriteLine($"Requesting with DELETE {route}");
            var response = await this.Fixture.HttpClient.DeleteAsync(route);

            // Assert
            response.StatusCode.Should()
                .Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}