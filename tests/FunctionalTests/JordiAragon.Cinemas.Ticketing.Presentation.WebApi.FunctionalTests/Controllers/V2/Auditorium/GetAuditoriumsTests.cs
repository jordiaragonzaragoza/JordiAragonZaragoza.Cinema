namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.FunctionalTests.Controllers.V2.Auditorium
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.FunctionalTests.Common;
    using Xunit;
    using Xunit.Abstractions;

    public class GetAuditoriumsTests : BaseWebApiFunctionalTests<AuditoriumsController>
    {
        public GetAuditoriumsTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAllAuditoriums_WhenHavingValidUrl_ShouldReturnOneAuditorium()
        {
            // Arrange
            var url = this.ControllerBaseRoute;

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<AuditoriumResponse>>(url, this.OutputHelper);

            // Assert
            response.Should()
                .NotBeNullOrEmpty()
                .And
                .HaveCount(1);
        }
    }
}