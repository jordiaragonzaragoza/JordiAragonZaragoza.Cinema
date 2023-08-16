namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Controllers.V2.Showtime
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    [Collection(nameof(SharedTestCollection))]
    public class GetShowtimesTests
    {
        private readonly HttpClient httpClient;

        public GetShowtimesTests(FunctionalTestsFixture<Program> fixture)
        {
            Guard.Against.Null(fixture, nameof(fixture));

            this.httpClient = fixture.HttpClient;
        }

        [Fact]
        public async Task GetAllShowtimes_WhenHavingValidUrl_ShouldReturnOneShowtime()
        {
            // Arrange
            var url = ControllerBaseExtensions.GetControllerBaseRoute<ShowtimesController>();
            url += "?auditoriumId=c91aa0e0-9bc0-4db3-805c-23e3d8eabf53&movieId=3fa85f64-5717-4562-b3fc-2c963f66afa6";

            // Act
            var response = await this.httpClient.GetAndDeserializeAsync<IEnumerable<ShowtimeResponse>>(url);

            // Assert
            response.Should()
                .NotBeNullOrEmpty()
                .And
                .HaveCount(1);
        }
    }
}