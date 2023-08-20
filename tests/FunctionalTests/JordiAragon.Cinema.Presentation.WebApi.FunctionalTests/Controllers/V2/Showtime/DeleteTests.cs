namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Controllers.V2.Showtime
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema;
    using JordiAragon.Cinema.Infrastructure.EntityFramework.AssemblyConfiguration;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common;
    using Xunit;
    using Xunit.Abstractions;

    public class DeleteTests : BaseWebApiFunctionalTests<ShowtimesController>
    {
        public DeleteTests(
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

            var route = this.ControllerBaseRoute + this.GetControllerMethodRoute(nameof(ShowtimesController.DeleteAsync));
            route = route.Replace("{showtimeId}", showtimeId);

            // Act
            var response = await this.Fixture.HttpClient.DeleteAndEnsureNoContentAsync(route, this.OutputHelper);

            // Assert
            response.StatusCode.Should()
                .Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}