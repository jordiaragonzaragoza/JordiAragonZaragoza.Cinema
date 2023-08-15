namespace JordiAragon.Cinema.WebApi.V2.FunctionalTests.MoviesController
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Movie.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinema.WebApi.V2.FunctionalTests.Common;
    using Xunit;

    [Collection("Sequential")]
    public class GetMoviesControllerTests : IClassFixture<FunctionalTestsFixture<Program>>
    {
        private readonly HttpClient httpClient;

        public GetMoviesControllerTests(FunctionalTestsFixture<Program> fixture)
        {
            this.httpClient = fixture.HttpClient;
        }

        [Fact]
        public async Task GetAllMovies_WhenHavingValidUrl_ShouldReturnOneMovie()
        {
            // Arrange
            var url = ControllerBaseExtensions.GetControllerBaseRoute<MoviesController>();

            // Act
            var response = await this.httpClient.GetAndDeserializeAsync<IEnumerable<MovieResponse>>(url);

            // Assert
            response.Should()
                .NotBeNullOrEmpty()
                .And
                .HaveCount(1);
        }
    }
}