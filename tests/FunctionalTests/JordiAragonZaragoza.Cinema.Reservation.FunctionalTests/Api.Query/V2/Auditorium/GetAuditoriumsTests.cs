namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.V2.Auditorium
{
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.Common;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GetAuditoriumsTests : BaseHttpRestfulApiFunctionalTests
    {
        public GetAuditoriumsTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAllAuditoriums_WhenHavingValidUrl_ShouldReturnOneAuditorium()
        {
            // Arrange
            var url = $"{Routes.ApiBase}{AuditoriumRoutes.GetAuditoriums}";

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<PaginatedCollectionResponse<AuditoriumResponse>>(url, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.Items.Should().HaveCount(1);
        }
    }
}