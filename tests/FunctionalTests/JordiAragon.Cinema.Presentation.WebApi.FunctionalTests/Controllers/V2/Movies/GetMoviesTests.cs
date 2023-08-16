namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Controllers.V2.Movies
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Movie.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common;
    using Xunit;

    [Collection("Sequential")]
    public class GetMoviesTests : IClassFixture<FunctionalTestsFixture<Program>>
    {
        private readonly HttpClient httpClient;

        public GetMoviesTests(FunctionalTestsFixture<Program> fixture)
        {
            Guard.Against.Null(fixture, nameof(fixture));

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