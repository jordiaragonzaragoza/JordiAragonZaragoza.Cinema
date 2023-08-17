namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Controllers.V2.Showtime
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Infrastructure.EntityFramework.AssemblyConfiguration;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;
    using Xunit.Abstractions;

    [Collection(nameof(SharedTestCollection))]
    public class GetShowtimesTests
    {
        private readonly HttpClient httpClient;
        private readonly ITestOutputHelper outputHelper;

        public GetShowtimesTests(FunctionalTestsFixture<Program> fixture, ITestOutputHelper outputHelper)
        {
            Guard.Against.Null(fixture, nameof(fixture));
            this.outputHelper = Guard.Against.Null(outputHelper, nameof(outputHelper));

            this.httpClient = fixture.CustomApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
        }

        [Fact]
        public async Task GetAllShowtimes_WhenHavingValidArguments_ShouldReturnOneShowtime()
        {
            var movieId = SeedData.ExampleMovie.Id.ToString();

            // Arrange
            var url = ControllerBaseExtensions.GetControllerBaseRoute<ShowtimesController>();
            url += $"?auditoriumId=c91aa0e0-9bc0-4db3-805c-23e3d8eabf53&movieId={movieId}";

            // Act
            var response = await this.httpClient.GetAndDeserializeAsync<IEnumerable<ShowtimeResponse>>(url, this.outputHelper);

            // Assert
            response.Should()
                .NotBeNullOrEmpty()
                .And
                .HaveCount(1);
        }

        [Fact]
        public async Task GetAllShowtimes_WhenHavingInValidArguments_ShouldReturnBadRequest()
        {
            // Arrange
            var url = ControllerBaseExtensions.GetControllerBaseRoute<ShowtimesController>();

            // Act
            var response = await this.httpClient.GetAndEnsureBadRequestAsync(url, this.outputHelper);

            // Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);
        }
    }
}