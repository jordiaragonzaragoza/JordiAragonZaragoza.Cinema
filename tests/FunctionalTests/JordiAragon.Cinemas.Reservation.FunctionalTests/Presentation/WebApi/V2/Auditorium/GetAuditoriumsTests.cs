namespace JordiAragon.Cinemas.Reservation.FunctionalTests.Presentation.WebApi.V2.Auditorium
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinemas.Reservation.Auditorium.Presentation.WebApi.V2;
    using JordiAragon.Cinemas.Reservation.FunctionalTests.Presentation.WebApi.Common;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using Xunit;
    using Xunit.Abstractions;

    public class GetAuditoriumsTests : BaseWebApiFunctionalTests
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
            var url = $"api/v2/{GetAuditoriums.Route}";

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<AuditoriumResponse>>(url, this.OutputHelper);

            // Assert
            response.Should()
                .NotBeNullOrEmpty()
                .And
                .HaveCount(1);
        }
    }
}