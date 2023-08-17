namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Controllers.V2.Movie
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Movie.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common;
    using Xunit;
    using Xunit.Abstractions;

    public class GetMoviesTests : BaseWebApiFunctionalTests
    {
        public GetMoviesTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        protected override string ControllerBasePath
        {
            get => ControllerRouteHelpers.GetControllerBasePath<MoviesController>();
        }

        [Fact]
        public async Task GetAllMovies_WhenHavingValidUrl_ShouldReturnOneMovie()
        {
            // Arrange
            var url = this.ControllerBasePath;

            // Act
            var response = await this.HttpClient.GetAndDeserializeAsync<IEnumerable<MovieResponse>>(url, this.OutputHelper);

            // Assert
            response.Should()
                .NotBeNullOrEmpty()
                .And
                .HaveCount(1);
        }
    }
}