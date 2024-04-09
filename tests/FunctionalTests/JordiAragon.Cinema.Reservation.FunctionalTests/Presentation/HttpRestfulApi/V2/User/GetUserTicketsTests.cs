namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2;
    using JordiAragon.Cinema.Reservation.User.Presentation.HttpRestfulApi.V2;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GetUserTicketsTests : BaseHttpRestfulApiFunctionalTests
    {
        public GetUserTicketsTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAllUserTickets_WhenHavingValidArguments_ShouldReturnOneTicket()
        {
            // Arrange
            var showtimeId = SeedData.ExampleShowtime.Id;

            var ticketResponse = await this.ReserveSeatsAsync(showtimeId);

            var userId = SeedData.ExampleUser.Id;

            // Required to satisfy eventual consistency on projections.
            await Task.Delay(TimeSpan.FromSeconds(2));

            var route = $"api/v2/{GetUserTickets.Route}";
            string pathAndQuery = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(userId), userId.ToString()));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<PaginatedCollectionResponse<TicketResponse>>(pathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.Items.Should().HaveCount(1);
            response.Items.First().Should().BeEquivalentTo(ticketResponse);
        }

        private async Task<TicketResponse> ReserveSeatsAsync(Guid showtimeId)
        {
            var routeAvailableSeats = $"api/v2/{GetAvailableSeats.Route}";
            routeAvailableSeats = routeAvailableSeats.Replace("{showtimeId}", showtimeId.ToString());

            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(routeAvailableSeats, this.OutputHelper);

            var seatsIds = availableSeatsResponse.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).Select(seat => seat.Id);

            var reserveSeatsRequest = new ReserveSeatsRequest(showtimeId, seatsIds);
            var reserveSeatsContent = StringContentHelpers.FromModelAsJson(reserveSeatsRequest);

            var reserveSeatsRoute = $"api/v2/{ReserveSeats.Route}";
            reserveSeatsRoute = reserveSeatsRoute.Replace("{showtimeId}", showtimeId.ToString());

            return await this.Fixture.HttpClient.PostAndDeserializeAsync<TicketResponse>(reserveSeatsRoute, reserveSeatsContent, this.OutputHelper);
        }
    }
}