namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.FunctionalTests.Controllers.V2.Showtime
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Infrastructure.EntityFramework.AssemblyConfiguration;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.FunctionalTests.Common;
    using Xunit;
    using Xunit.Abstractions;

    public class GetAvailableSeatsTests : BaseWebApiFunctionalTests<ShowtimesController>
    {
        public GetAvailableSeatsTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAvailableSeats_WhenHavingValidArguments_ShouldReturnAvailableSeats()
        {
            // Arrange
            var showtimeId = SeedData.ExampleShowtime.Id.ToString();

            var route = this.ControllerBaseRoute + this.GetControllerMethodRoute(nameof(ShowtimesController.GetAvailableSeatsAsync));
            route = route.Replace("{showtimeId}", showtimeId);

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(route, this.OutputHelper);

            // Assert
            response.Should()
                .NotBeNullOrEmpty();
        }
    }
}