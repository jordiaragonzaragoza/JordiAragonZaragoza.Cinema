namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Controllers.V2.Showtime
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema;
    using JordiAragon.Cinema.Infrastructure.EntityFramework.AssemblyConfiguration;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common;
    using Xunit;
    using Xunit.Abstractions;

    public class CreateTests : BaseWebApiFunctionalTests<ShowtimesController>
    {
        public CreateTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task CreateShowtime_WhenHavingValidArguments_ShouldCreateRequiredShowtime()
        {
            // Arrange
            var request = new CreateShowtimeRequest(
                SeedData.ExampleAuditorium.Id,
                SeedData.ExampleMovie.Id,
                DateTime.UtcNow.AddDays(1));

            var content = StringContentHelpers.FromModelAsJson(request);

            // Act
            var response = await this.Fixture.HttpClient.PostAndDeserializeAsync<Guid>(this.ControllerBaseRoute, content, this.OutputHelper);

            // Assert
            response.Should()
                .NotBeEmpty();
        }
    }
}