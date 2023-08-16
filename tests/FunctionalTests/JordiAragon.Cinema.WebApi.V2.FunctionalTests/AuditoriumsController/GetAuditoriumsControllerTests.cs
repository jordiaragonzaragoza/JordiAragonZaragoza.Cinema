namespace JordiAragon.Cinema.WebApi.V2.FunctionalTests.AuditoriumsController
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinema.WebApi.V2.FunctionalTests.Common;
    using Xunit;

    [Collection("Sequential")]
    public class GetAuditoriumsControllerTests : IClassFixture<FunctionalTestsFixture<Program>>
    {
        private readonly HttpClient httpClient;

        public GetAuditoriumsControllerTests(FunctionalTestsFixture<Program> fixture)
        {
            Guard.Against.Null(fixture, nameof(fixture));

            this.httpClient = fixture.HttpClient;
        }

        [Fact]
        public async Task GetAllAuditoriums_WhenHavingValidUrl_ShouldReturnThreeAuditoriums()
        {
            // Arrange
            var url = ControllerBaseExtensions.GetControllerBaseRoute<AuditoriumsController>();

            // Act
            var response = await this.httpClient.GetAndDeserializeAsync<IEnumerable<AuditoriumResponse>>(url);

            // Assert
            response.Should()
                .NotBeNullOrEmpty()
                .And
                .HaveCount(3);
        }
    }
}