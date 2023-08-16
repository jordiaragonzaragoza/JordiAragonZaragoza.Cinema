namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Controllers.V2.Auditorium
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    [Collection(nameof(SharedTestCollection))]
    public class GetAuditoriumsTests
    {
        private readonly HttpClient httpClient;

        public GetAuditoriumsTests(FunctionalTestsFixture<Program> fixture)
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