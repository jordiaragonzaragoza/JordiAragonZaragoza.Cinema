namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.V2.Movie
{
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Movie;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Movie.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.Common;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GetMoviesTests : BaseHttpRestfulApiFunctionalTests
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
            var url = $"{Routes.ApiBase}{MovieRoutes.GetMovies}";

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<PaginatedCollectionResponse<MovieResponse>>(url, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.Items.Should().HaveCount(1);
        }
    }
}