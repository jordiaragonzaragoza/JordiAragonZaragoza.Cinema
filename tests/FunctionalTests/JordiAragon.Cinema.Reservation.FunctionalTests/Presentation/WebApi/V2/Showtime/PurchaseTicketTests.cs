namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.V2.Showtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2;
    using JordiAragon.Cinema.Reservation.User.Presentation.WebApi.V2;
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

            var routeReserveSeats = $"api/v2/{ReserveSeats.Route}";
            routeReserveSeats = routeReserveSeats.Replace("{showtimeId}", showtimeId.ToString());

            var ticketReserveResponse = await this.Fixture.HttpClient.PostAndDeserializeAsync<TicketResponse>(routeReserveSeats, reserveSeatsContent, this.OutputHelper);

            var ticketId = ticketReserveResponse.Id;

            var routePurchaseTicket = $"api/v2/{PurchaseTicket.Route}";
            routePurchaseTicket = routePurchaseTicket.Replace("{showtimeId}", showtimeId.ToString());
            routePurchaseTicket = routePurchaseTicket.Replace("{ticketId}", ticketId.ToString());

            var purchaseTicketRequest = new PurchaseTicketRequest(showtimeId, ticketReserveResponse.Id, true);
            var purchaseTicketContent = StringContentHelpers.FromModelAsJson(purchaseTicketRequest);

            // Act
            this.OutputHelper.WriteLine($"Requesting with PATCH {routePurchaseTicket}");
            var response = await this.Fixture.HttpClient.PatchAsync(routePurchaseTicket, purchaseTicketContent);

            // Assert
            response.StatusCode.Should()
               .Be(HttpStatusCode.NoContent);

            await this.TestProjectionsAsync(showtimeId, seatsIds, ticketId);
        }

        private async Task TestProjectionsAsync(Guid showtimeId, IEnumerable<Guid> seatsIds, Guid ticketId)
        {
            // Required to satisfy eventual consistency on projections.
            await Task.Delay(TimeSpan.FromSeconds(2));

            await this.GetUserTicket_WhenTicketPurchased_ShouldReturnTicketPurchased(showtimeId, seatsIds, ticketId);
        }

        private async Task GetUserTicket_WhenTicketPurchased_ShouldReturnTicketPurchased(Guid showtimeId, IEnumerable<Guid> seatsIds, Guid ticketId)
        {
            var userId = SeedData.ExampleUser.Id;
            var routeUserTicket = $"api/v2/{GetUserTicket.Route}";
            routeUserTicket = routeUserTicket.Replace("{userId}", userId.ToString());
            routeUserTicket = routeUserTicket.Replace("{showtimeId}", showtimeId.ToString());
            routeUserTicket = routeUserTicket.Replace("{ticketId}", ticketId.ToString());

            var ticketPurchasedResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<TicketResponse>(routeUserTicket, this.OutputHelper);

            ticketPurchasedResponse.SessionDateOnUtc.Should()
                .Be(SeedData.ExampleShowtime.SessionDateOnUtc);

            ticketPurchasedResponse.AuditoriumName.Should()
                .Be(SeedData.ExampleAuditorium.Name);

            ticketPurchasedResponse.MovieTitle.Should()
                .Be(SeedData.ExampleMovie.Title);

            ticketPurchasedResponse.Seats.Select(seatResponse => seatResponse.Id).Should()
                .Contain(seatsIds);

            ticketPurchasedResponse.IsPurchased.Should().BeTrue();
        }
    }
}