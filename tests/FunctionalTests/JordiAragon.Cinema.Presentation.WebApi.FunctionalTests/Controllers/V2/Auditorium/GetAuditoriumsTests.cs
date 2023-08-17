namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Controllers.V2.Auditorium
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common;
    using Xunit;
    using Xunit.Abstractions;

    public class GetAuditoriumsTests : BaseWebApiFunctionalTests
    {
        public GetAuditoriumsTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        protected override string ControllerBasePath
        {
            get => ControllerRouteHelpers.GetControllerBasePath<AuditoriumsController>();
        }

        [Fact]
        public async Task GetAllAuditoriums_WhenHavingValidUrl_ShouldReturnOneAuditorium()
        {
            // Arrange
            var url = this.ControllerBasePath;

            // Act
            var response = await this.HttpClient.GetAndDeserializeAsync<IEnumerable<AuditoriumResponse>>(url, this.OutputHelper);

            // Assert
            response.Should()
                .NotBeNullOrEmpty()
                .And
                .HaveCount(1);
        }
    }
}