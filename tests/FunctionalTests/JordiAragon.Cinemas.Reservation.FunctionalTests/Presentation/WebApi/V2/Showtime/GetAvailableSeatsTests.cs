namespace JordiAragon.Cinemas.Reservation.FunctionalTests.Presentation.WebApi.V2.Showtime
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinemas.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinemas.Reservation.FunctionalTests.Presentation.WebApi.Common;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinemas.Reservation.Showtime.Presentation.WebApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    public class GetAvailableSeatsTests : BaseWebApiFunctionalTests
    {
        public GetAvailableSeatsTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAvailableSeats_WhenHavingValidArguments_ShouldReturnAvailableSeats()
        {
            // Arrange
            var showtimeId = SeedData.ExampleShowtime.Id.ToString();

            var route = $"api/v2/{GetAvailableSeats.Route}";
            route = route.Replace("{showtimeId}", showtimeId);

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(route, this.OutputHelper);

            // Assert
            response.Should()
                .NotBeNullOrEmpty();
        }
    }
}