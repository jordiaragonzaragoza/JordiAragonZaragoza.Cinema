namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.HttpRestfulApi.V2.Auditorium
{
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GetAuditoriumsTests : BaseSystemTests
    {
        public GetAuditoriumsTests(
            SystemTestsFixture fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAllAuditoriums_WhenHavingValidUrl_ShouldReturnOneAuditorium()
        {
            // Arrange
            var url = $"api/v2/{GetAuditoriumsRequest.Route}";

            // Act
            var response = await this.Fixture.ReservationHttpClient.GetAndDeserializeAsync<PaginatedCollectionResponse<AuditoriumResponse>>(url, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.Items.Should().HaveCount(1);
        }
    }
}