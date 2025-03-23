namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.Showtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    using Constants = JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain.Constants;

    public sealed class ScheduleShowtimeTests : BaseHttpRestfulApiFunctionalTests
    {
        public ScheduleShowtimeTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task ScheduleShowtime_WhenHavingValidArguments_ShouldCreateRequiredShowtime()
        {
            // Arrange
            var showtimeId = Guid.NewGuid();

            var route = $"api/v2/{ScheduleShowtime.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            var sessionDateOnUtc = DateTimeOffset.UtcNow.AddDays(1);

            var request = new ScheduleShowtimeRequest(
                showtimeId,
                Constants.Auditorium.Id,
                Constants.Movie.Id,
                sessionDateOnUtc);

            var content = StringContentHelpers.FromModelAsJson(request);

            var fullUri = new Uri(this.Fixture.HttpClient.BaseAddress!, route);

            // Act
            this.OutputHelper.WriteLine($"Requesting with PUT {route}");
            await this.Fixture.HttpClient.PutAsync(fullUri, content);

            await AddEventualConsistencyDelayAsync();

            // Assert
            await this.TestProjectionsAsync(sessionDateOnUtc, showtimeId);
        }

        private async Task TestProjectionsAsync(DateTimeOffset sessionDateOnUtc, Guid showtimeId)
        {
            await this.GetShowtime_WhenShowtimeCreated_ShouldReturnShowtimeCreated(sessionDateOnUtc, showtimeId);

            await this.GetAvailableSeats_WhenShowtimeCreated_ShouldReturnAvailableSeats(showtimeId);
        }

        private async Task GetShowtime_WhenShowtimeCreated_ShouldReturnShowtimeCreated(DateTimeOffset sessionDateOnUtc, Guid showtimeId)
        {
            // Arrange
            var getShowtimeRoute = $"api/v2/{GetShowtime.Route}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                getShowtimeRoute,
                (nameof(showtimeId), showtimeId.ToString()));

            // Act
            var showtimeResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<ShowtimeResponse>(uri.PathAndQuery, this.OutputHelper);

            // Assert
            showtimeResponse.Should().NotBeNull();
            showtimeResponse.MovieTitle.Should().Be(SeedData.ExampleMovie.Title);
            showtimeResponse.SessionDateOnUtc.Should().Be(sessionDateOnUtc);
            showtimeResponse.AuditoriumId.Should().Be(SeedData.ExampleAuditorium.Id);
            showtimeResponse.AuditoriumName.Should().Be(SeedData.ExampleAuditorium.Name);
        }

        private async Task GetAvailableSeats_WhenShowtimeCreated_ShouldReturnAvailableSeats(Guid showtimeId)
        {
            // Arrange
            var route = $"api/v2/{GetAvailableSeats.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            // Act
            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(route, this.OutputHelper);

            // Assert
            availableSeatsResponse.Should().NotBeNullOrEmpty();
            availableSeatsResponse.Count().Should().Be(SeedData.ExampleAuditorium.Rows * SeedData.ExampleAuditorium.SeatsPerRow);
        }
    }
}