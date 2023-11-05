namespace JordiAragon.Cinemas.Reservation.FunctionalTests.Presentation.WebApi.V2.Movie
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinemas.Reservation.FunctionalTests.Presentation.WebApi.Common;
    using JordiAragon.Cinemas.Reservation.Movie.Presentation.WebApi.V2;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Movie.Responses;
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

        [Fact]
        public async Task GetAllMovies_WhenHavingValidUrl_ShouldReturnOneMovie()
        {
            // Arrange
            var url = $"api/v2/{GetMovies.Route}";

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