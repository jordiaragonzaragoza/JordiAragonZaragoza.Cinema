namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Controllers.V2.Showtime
{
    using System.Collections.Generic;
    using System.Linq;
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

    public class CreateTicketTests : BaseWebApiFunctionalTests<ShowtimesController>
    {
        public CreateTicketTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task CreateTicket_WhenHavingValidArguments_ShouldCreateRequiredTicket()
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

            var route = this.ControllerBaseRoute + this.GetControllerMethodRoute(nameof(ShowtimesController.CreateTicketAsync));
            route = route.Replace("{showtimeId}", showtimeId);

            // Act
            var ticketResponse = await this.Fixture.HttpClient.PostAndDeserializeAsync<TicketResponse>(route, content, this.OutputHelper);

            // Assert
            ticketResponse.SessionDateOnUtc.Should()
                .Be(SeedData.ExampleShowtime.SessionDateOnUtc);

            ticketResponse.Auditorium.Should()
                .Be(SeedData.ExampleShowtime.AuditoriumId);

            ticketResponse.MovieName.Should()
                .Be(SeedData.ExampleMovie.Title);

            ticketResponse.Seats.Select(seatResponse => seatResponse.Id).Should()
                .Contain(seatsIds);
        }
    }
}