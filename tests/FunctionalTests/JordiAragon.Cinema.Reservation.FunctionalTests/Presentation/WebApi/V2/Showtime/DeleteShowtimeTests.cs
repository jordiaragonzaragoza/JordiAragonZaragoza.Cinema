namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.V2.Showtime
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    public class DeleteShowtimeTests : BaseWebApiFunctionalTests
    {
        public DeleteShowtimeTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task DeleteShowtime_WhenHavingValidArguments_ShouldDeleteRequiredShowtime()
        {
            // Arrange
            var showtimeId = await this.CreateNewShowtimeAsync();

            var route = $"api/v2/{DeleteShowtime.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString());

            // Act
            this.OutputHelper.WriteLine($"Requesting with DELETE {route}");
            var response = await this.Fixture.HttpClient.DeleteAsync(route);

            // Assert
            response.StatusCode.Should()
                .Be(System.Net.HttpStatusCode.NoContent);

            await this.TestProjectionsAsync(showtimeId);
        }

        private async Task TestProjectionsAsync(Guid showtimeId)
        {
            // Required to satisfy eventual consistency on projections.
            await Task.Delay(TimeSpan.FromSeconds(2));

            await this.GetShowtime_WhenShowtimeDeleted_ShouldReturnNotFound(showtimeId);

            await this.GetAvailableSeats_WhenShowtimeDeleted_ShouldReturnNotFound(showtimeId);

            await this.GetShowtimeTickets_WhenShowtimeDeleted_ShouldReturnNotFound(showtimeId);
        }

        private async Task GetShowtime_WhenShowtimeDeleted_ShouldReturnNotFound(Guid showtimeId)
        {
            // Arrange
            var getShowtimeRoute = $"api/v2/{GetShowtime.Route}";
            string pathAndQuery = EndpointRouteHelpers.BuildUriWithQueryParameters(
                getShowtimeRoute,
                (nameof(showtimeId), showtimeId.ToString()));

            // Act
            var showtimeResponse = await this.Fixture.HttpClient.GetAndEnsureNotFoundAsync(pathAndQuery, this.OutputHelper);

            // Assert
            showtimeResponse.StatusCode.Should()
                .Be(System.Net.HttpStatusCode.NotFound);
        }

        private async Task GetAvailableSeats_WhenShowtimeDeleted_ShouldReturnNotFound(Guid showtimeId)
        {
            // Arrange
            var route = $"api/v2/{GetAvailableSeats.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString());

            // Act
            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndEnsureNotFoundAsync(route, this.OutputHelper);

            // Assert
            availableSeatsResponse.StatusCode.Should()
                .Be(System.Net.HttpStatusCode.NotFound);
        }

        private async Task GetShowtimeTickets_WhenShowtimeDeleted_ShouldReturnNotFound(Guid showtimeId)
        {
            // Arrange
            var route = $"api/v2/{GetShowtimeTickets.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString());

            // Act
            var showtimeTicketsResponse = await this.Fixture.HttpClient.GetAndEnsureNotFoundAsync(route, this.OutputHelper);

            // Assert
            showtimeTicketsResponse.StatusCode.Should()
                .Be(System.Net.HttpStatusCode.NotFound);
        }

        private async Task<Guid> CreateNewShowtimeAsync()
        {
            var url = $"api/v2/{CreateShowtime.Route}";

            var sessionDateOnUtc = DateTimeOffset.UtcNow.AddDays(1);

            var request = new CreateShowtimeRequest(
                SeedData.ExampleAuditorium.Id,
                SeedData.ExampleMovie.Id,
                sessionDateOnUtc);

            var content = StringContentHelpers.FromModelAsJson(request);

            // Act
            var showtimeId = await this.Fixture.HttpClient.PostAndDeserializeAsync<Guid>(url, content, this.OutputHelper);

            // Required to satisfy eventual consistency on projections.
            await Task.Delay(TimeSpan.FromSeconds(2));

            return showtimeId;
        }
    }
}