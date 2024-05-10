namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.Showtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2;
    using Xunit;
    using Xunit.Abstractions;

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
            var url = $"api/v2/{ScheduleShowtime.Route}";

            var sessionDateOnUtc = DateTimeOffset.UtcNow.AddDays(1);

            var request = new ScheduleShowtimeRequest(
                SeedData.ExampleAuditorium.Id,
                SeedData.ExampleMovie.Id,
                sessionDateOnUtc);

            var content = StringContentHelpers.FromModelAsJson(request);

            // Act
            var showtimeId = await this.Fixture.HttpClient.PostAndDeserializeAsync<Guid>(url, content, this.OutputHelper);

            // Assert
            showtimeId.Should()
                .NotBeEmpty();

            await this.TestProjectionsAsync(sessionDateOnUtc, showtimeId);
        }

        private async Task TestProjectionsAsync(DateTimeOffset sessionDateOnUtc, Guid showtimeId)
        {
            // Required to satisfy eventual consistency on projections.
            await AddEventualConsistencyDelayAsync();

            await this.GetShowtime_WhenShowtimeCreated_ShouldReturnShowtimeCreated(sessionDateOnUtc, showtimeId);

            await this.GetAvailableSeats_WhenShowtimeCreated_ShouldReturnAvailableSeats(showtimeId);
        }

        private async Task GetShowtime_WhenShowtimeCreated_ShouldReturnShowtimeCreated(DateTimeOffset sessionDateOnUtc, Guid showtimeId)
        {
            // Arrange
            var getShowtimeRoute = $"api/v2/{GetShowtime.Route}";
            string pathAndQuery = EndpointRouteHelpers.BuildUriWithQueryParameters(
                getShowtimeRoute,
                (nameof(showtimeId), showtimeId.ToString()));

            // Act
            var showtimeResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<ShowtimeResponse>(pathAndQuery, this.OutputHelper);

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
            route = route.Replace("{showtimeId}", showtimeId.ToString());

            // Act
            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(route, this.OutputHelper);

            // Assert
            availableSeatsResponse.Should().NotBeNullOrEmpty();
            availableSeatsResponse.Count().Should().Be(SeedData.ExampleAuditorium.Rows * SeedData.ExampleAuditorium.SeatsPerRow);
        }
    }
}