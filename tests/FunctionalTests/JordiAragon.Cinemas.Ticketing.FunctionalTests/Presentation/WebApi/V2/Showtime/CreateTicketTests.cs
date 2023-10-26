namespace JordiAragon.Cinemas.Ticketing.FunctionalTests.Presentation.WebApi.V2.Showtime
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing;
    using JordiAragon.Cinemas.Ticketing.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinemas.Ticketing.FunctionalTests.Presentation.WebApi.Common;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinemas.Ticketing.Showtime.Presentation.WebApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    public class CreateTicketTests : BaseWebApiFunctionalTests
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
            var showtimeId = SeedData.ExampleShowtime.Id;

            var routeAvailableSeats = $"api/v2/{GetAvailableSeats.Route}";
            routeAvailableSeats = routeAvailableSeats.Replace("{showtimeId}", showtimeId.ToString());

            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(routeAvailableSeats, this.OutputHelper);

            var seatsIds = availableSeatsResponse.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).Select(seat => seat.Id);

            var request = new CreateTicketRequest(showtimeId, seatsIds);
            var content = StringContentHelpers.FromModelAsJson(request);

            var route = $"api/v2/{CreateTicket.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString());

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