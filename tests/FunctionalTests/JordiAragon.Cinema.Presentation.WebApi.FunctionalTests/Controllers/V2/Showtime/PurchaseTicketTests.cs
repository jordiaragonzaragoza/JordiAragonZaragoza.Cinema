namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Controllers.V2.Showtime
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema;
    using JordiAragon.Cinema.Infrastructure.EntityFramework.AssemblyConfiguration;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common;
    using Xunit;
    using Xunit.Abstractions;

    public class PurchaseTicketTests : BaseWebApiFunctionalTests<ShowtimesController>
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
            var showtimeId = SeedData.ExampleShowtime.Id.ToString();

            var routeAvailableSeats = this.ControllerBaseRoute + this.GetControllerMethodRoute(nameof(ShowtimesController.GetAvailableSeatsAsync));
            routeAvailableSeats = routeAvailableSeats.Replace("{showtimeId}", showtimeId);

            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(routeAvailableSeats, this.OutputHelper);

            var seatsIds = availableSeatsResponse.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).Select(seat => seat.Id);

            var request = new CreateTicketRequest(seatsIds);
            var content = StringContentHelpers.FromModelAsJson(request);

            var routeCreateTicket = this.ControllerBaseRoute + this.GetControllerMethodRoute(nameof(ShowtimesController.CreateTicketAsync));
            routeCreateTicket = routeCreateTicket.Replace("{showtimeId}", showtimeId);

            var ticketResponse = await this.Fixture.HttpClient.PostAndDeserializeAsync<TicketResponse>(routeCreateTicket, content, this.OutputHelper);

            var ticketId = ticketResponse.TicketId.ToString();

            var route = this.ControllerBaseRoute + this.GetControllerMethodRoute(nameof(ShowtimesController.PurchaseTicketAsync));
            route = route.Replace("{showtimeId}", showtimeId);
            route = route.Replace("{ticketId}", ticketId);

            // Act
            this.OutputHelper.WriteLine($"Requesting with PATCH {route}");
            var response = await this.Fixture.HttpClient.PatchAsync(route, content: null);

            // Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.OK);
        }
    }
}