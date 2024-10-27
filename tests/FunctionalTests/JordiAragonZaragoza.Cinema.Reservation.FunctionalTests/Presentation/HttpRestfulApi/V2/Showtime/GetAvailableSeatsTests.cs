namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.Showtime
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    using Constants = JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain.Constants;

    public sealed class GetAvailableSeatsTests : BaseHttpRestfulApiFunctionalTests
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
            var showtimeId = await this.ScheduleNewShowtimeAsync();

            var route = $"api/v2/{GetAvailableSeats.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(route, this.OutputHelper);

            // Assert
            response.Should()
                .NotBeNullOrEmpty();
        }

        private async Task<Guid> ScheduleNewShowtimeAsync()
        {
            var url = $"api/v2/{ScheduleShowtime.Route}";

            var sessionDateOnUtc = DateTimeOffset.UtcNow.AddDays(1);

            var request = new ScheduleShowtimeRequest(
                Constants.Auditorium.Id,
                Constants.Movie.Id,
                sessionDateOnUtc);

            var content = StringContentHelpers.FromModelAsJson(request);

            var response = await this.Fixture.HttpClient.PostAndDeserializeAsync<Guid>(url, content, this.OutputHelper);

            await AddEventualConsistencyDelayAsync();

            return response;
        }
    }
}