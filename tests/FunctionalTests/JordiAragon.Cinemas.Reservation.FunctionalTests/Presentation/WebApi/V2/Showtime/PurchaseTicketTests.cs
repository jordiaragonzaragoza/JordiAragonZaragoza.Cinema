namespace JordiAragon.Cinemas.Reservation.FunctionalTests.Presentation.WebApi.V2.Showtime
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinemas.Reservation;
    using JordiAragon.Cinemas.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinemas.Reservation.FunctionalTests.Presentation.WebApi.Common;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinemas.Reservation.Showtime.Presentation.WebApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    public class PurchaseTicketTests : BaseWebApiFunctionalTests
    {
        public PurchaseTicketTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task PurchaseTicket_WhenHavingValidArguments_ShouldMarkTicketAsPaid()
        {
            // Arrange
            var showtimeId = SeedData.ExampleShowtime.Id;

            var routeAvailableSeats = $"api/v2/{GetAvailableSeats.Route}";
            routeAvailableSeats = routeAvailableSeats.Replace("{showtimeId}", showtimeId.ToString());

            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(routeAvailableSeats, this.OutputHelper);

            var seatsIds = availableSeatsResponse.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).Select(seat => seat.Id);

            var reserveSeatsRequest = new ReserveSeatsRequest(showtimeId, seatsIds);
            var reserveSeatsContent = StringContentHelpers.FromModelAsJson(reserveSeatsRequest);

            var routeCreateTicket = $"api/v2/{ReserveSeats.Route}";
            routeCreateTicket = routeCreateTicket.Replace("{showtimeId}", showtimeId.ToString());

            var ticketResponse = await this.Fixture.HttpClient.PostAndDeserializeAsync<TicketResponse>(routeCreateTicket, reserveSeatsContent, this.OutputHelper);

            var ticketId = ticketResponse.TicketId.ToString();

            var route = $"api/v2/{PurchaseTicket.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString());
            route = route.Replace("{ticketId}", ticketId);

            var purchaseTicketRequest = new PurchaseTicketRequest(showtimeId, ticketResponse.TicketId, true);
            var purchaseTicketContent = StringContentHelpers.FromModelAsJson(purchaseTicketRequest);

            // Act
            var response = await this.Fixture.HttpClient.PatchAndDeserializeAsync<TicketResponse>(route, purchaseTicketContent, this.OutputHelper);

            // Assert
            response.IsPurchased.Should().BeTrue();
        }
    }
}