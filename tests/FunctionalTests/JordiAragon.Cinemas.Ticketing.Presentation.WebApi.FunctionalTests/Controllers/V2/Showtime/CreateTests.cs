namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.FunctionalTests.Controllers.V2.Showtime
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing;
    using JordiAragon.Cinemas.Ticketing.Infrastructure.EntityFramework.AssemblyConfiguration;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.FunctionalTests.Common;
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