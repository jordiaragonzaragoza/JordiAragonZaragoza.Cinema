namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.Showtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using Xunit;
    using Xunit.Abstractions;

    using Constants = JordiAragon.Cinema.Reservation.TestUtilities.Domain.Constants;

    public sealed class GetShowtimeTicketsTests : BaseHttpRestfulApiFunctionalTests
    {
        public GetShowtimeTicketsTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAllShowtimeTickets_WhenHavingValidArguments_ShouldReturnOneTicket()
        {
            // Arrange
            var showtimeId = await this.ScheduleNewShowtimeAsync();

            var ticketResponse = await this.ReserveSeatsAsync(showtimeId);

            var route = $"api/v2/{GetShowtimeTickets.Route}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(showtimeId), showtimeId.ToString()));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<PaginatedCollectionResponse<TicketResponse>>(uri.PathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.Items.Should().HaveCount(1);
            response.Items.First().Should().BeEquivalentTo(ticketResponse);
        }

        private async Task<TicketResponse> ReserveSeatsAsync(Guid showtimeId)
        {
            var routeAvailableSeats = $"api/v2/{GetAvailableSeats.Route}";
            routeAvailableSeats = routeAvailableSeats.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(routeAvailableSeats, this.OutputHelper);

            var seatsIds = availableSeatsResponse.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).Select(seat => seat.Id);

            var reserveSeatsRequest = new ReserveSeatsRequest(showtimeId, seatsIds);
            var reserveSeatsContent = StringContentHelpers.FromModelAsJson(reserveSeatsRequest);

            var reserveSeatsRoute = $"api/v2/{ReserveSeats.Route}";
            reserveSeatsRoute = reserveSeatsRoute.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            var response = await this.Fixture.HttpClient.PostAndDeserializeAsync<TicketResponse>(reserveSeatsRoute, reserveSeatsContent, this.OutputHelper);

            await AddEventualConsistencyDelayAsync();

            return response;
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