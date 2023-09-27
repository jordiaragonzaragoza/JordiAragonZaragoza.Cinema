namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.FunctionalTests.Controllers.V2.Movie
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Movie.Responses;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.FunctionalTests.Common;
    using Xunit;
    using Xunit.Abstractions;

    public class GetMoviesTests : BaseWebApiFunctionalTests<MoviesController>
    {
        public GetMoviesTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAllMovies_WhenHavingValidUrl_ShouldReturnOneMovie()
        {
            // Arrange
            var url = this.ControllerBaseRoute;

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<MovieResponse>>(url, this.OutputHelper);

            // Assert
            response.Should()
                .NotBeNullOrEmpty()
                .And
                .HaveCount(1);
        }
    }
}