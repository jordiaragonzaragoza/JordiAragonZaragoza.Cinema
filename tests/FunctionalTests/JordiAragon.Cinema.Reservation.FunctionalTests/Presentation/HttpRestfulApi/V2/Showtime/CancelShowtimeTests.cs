namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.Showtime
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class CancelShowtimeTests : BaseHttpRestfulApiFunctionalTests
    {
        public CancelShowtimeTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task CancelShowtime_WhenHavingValidArguments_ShouldDeleteRequiredShowtime()
        {
            // Arrange
            var showtimeId = await this.CreateNewShowtimeAsync();

            var route = $"api/v2/{CancelShowtime.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            var fullUri = new Uri(this.Fixture.HttpClient.BaseAddress!, route);

            // Act
            this.OutputHelper.WriteLine($"Requesting with DELETE {route}");
            var response = await this.Fixture.HttpClient.DeleteAsync(fullUri);

            await AddEventualConsistencyDelayAsync();

            // Assert
            response.StatusCode.Should()
                .Be(System.Net.HttpStatusCode.NoContent);

            await this.TestProjectionsAsync(showtimeId);
        }

        private async Task TestProjectionsAsync(Guid showtimeId)
        {
            await this.GetShowtime_WhenShowtimeCanceled_ShouldReturnNotFound(showtimeId);

            await this.GetAvailableSeats_WhenShowtimeCanceled_ShouldReturnNotFound(showtimeId);

            await this.GetShowtimeTickets_WhenShowtimeCanceled_ShouldReturnNotFound(showtimeId);
        }

        private async Task GetShowtime_WhenShowtimeCanceled_ShouldReturnNotFound(Guid showtimeId)
        {
            // Arrange
            var getShowtimeRoute = $"api/v2/{GetShowtime.Route}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                getShowtimeRoute,
                (nameof(showtimeId), showtimeId.ToString()));

            // Act
            var showtimeResponse = await this.Fixture.HttpClient.GetAndEnsureNotFoundAsync(uri.PathAndQuery, this.OutputHelper);

            // Assert
            showtimeResponse.StatusCode.Should()
                .Be(System.Net.HttpStatusCode.NotFound);
        }

        private async Task GetAvailableSeats_WhenShowtimeCanceled_ShouldReturnNotFound(Guid showtimeId)
        {
            // Arrange
            var route = $"api/v2/{GetAvailableSeats.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            // Act
            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndEnsureNotFoundAsync(route, this.OutputHelper);

            // Assert
            availableSeatsResponse.StatusCode.Should()
                .Be(System.Net.HttpStatusCode.NotFound);
        }

        private async Task GetShowtimeTickets_WhenShowtimeCanceled_ShouldReturnNotFound(Guid showtimeId)
        {
            // Arrange
            var route = $"api/v2/{GetShowtimeTickets.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            // Act
            var showtimeTicketsResponse = await this.Fixture.HttpClient.GetAndEnsureNotFoundAsync(route, this.OutputHelper);

            // Assert
            showtimeTicketsResponse.StatusCode.Should()
                .Be(System.Net.HttpStatusCode.NotFound);
        }

        private async Task<Guid> CreateNewShowtimeAsync()
        {
            var url = $"api/v2/{ScheduleShowtime.Route}";

            var sessionDateOnUtc = DateTimeOffset.UtcNow.AddDays(1);

            var request = new ScheduleShowtimeRequest(
                SeedData.ExampleAuditorium.Id,
                SeedData.ExampleMovie.Id,
                sessionDateOnUtc);

            var content = StringContentHelpers.FromModelAsJson(request);

            var response = await this.Fixture.HttpClient.PostAndDeserializeAsync<Guid>(url, content, this.OutputHelper);

            await AddEventualConsistencyDelayAsync();

            return response;
        }
    }
}