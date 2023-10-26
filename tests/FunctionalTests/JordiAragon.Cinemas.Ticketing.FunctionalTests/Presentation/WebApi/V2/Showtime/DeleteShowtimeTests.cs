namespace JordiAragon.Cinemas.Ticketing.FunctionalTests.Presentation.WebApi.V2.Showtime
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing;
    using JordiAragon.Cinemas.Ticketing.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinemas.Ticketing.FunctionalTests.Presentation.WebApi.Common;
    using JordiAragon.Cinemas.Ticketing.Showtime.Presentation.WebApi.V2;
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
                .Be(System.Net.HttpStatusCode.OK);
        }
    }
}