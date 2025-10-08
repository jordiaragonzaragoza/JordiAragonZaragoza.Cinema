namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.HttpRestfulApi.V2.Movie
{
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Movie.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Movie.Responses;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;

    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    using Xunit;
    using Xunit.Abstractions;

    public sealed class GetMoviesTests : BaseSystemTests
    {
        public GetMoviesTests(
            SystemTestsFixture fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAllMovies_WhenHavingValidUrl_ShouldReturnOneMovie()
        {
            // Arrange
            var url = $"api/v2/{GetMoviesRequest.Route}";

            // Act
            var response = await this.Fixture.ReservationHttpClient.GetAndDeserializeAsync<PaginatedCollectionResponse<MovieResponse>>(url, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.Items.Should().HaveCount(1);
        }
    }
}