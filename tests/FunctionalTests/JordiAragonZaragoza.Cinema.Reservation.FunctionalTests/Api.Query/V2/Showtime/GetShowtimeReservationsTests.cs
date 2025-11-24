namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.V2.Showtime
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.Common;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GetShowtimeReservationsTests : BaseHttpRestfulApiFunctionalTests
    {
        public GetShowtimeReservationsTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAllShowtimeReservations_WhenHavingValidArguments_ShouldReturnOneReservation()
        {
            // Arrange
            Guid showtimeId = SeedData.ExampleShowtime.Id;

            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetShowtimeReservations}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(showtimeId), showtimeId.ToString()));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<PaginatedCollectionResponse<ReservationResponse>>(uri.PathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.Items.Should().HaveCount(1);
        }

        /*private async Task<ReservationResponse> ReserveSeatsAsync(Guid showtimeId)
        {
            var routeAvailableSeats = $"api/v2/{GetAvailableSeatsRequest.Route}";
            routeAvailableSeats = routeAvailableSeats.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(routeAvailableSeats, this.OutputHelper);

            var seatsIds = availableSeatsResponse.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).Select(seat => seat.Id);
            var reservationId = Guid.NewGuid();
            var reserveSeatsRequest = new ReserveSeatsRequest(reservationId, showtimeId, seatsIds);
            var reserveSeatsContent = StringContentHelpers.FromModelAsJson(reserveSeatsRequest);

            var reserveSeatsRoute = $"api/v2/{ReserveSeatsRequest.Route}";
            reserveSeatsRoute = reserveSeatsRoute.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);
            reserveSeatsRoute = reserveSeatsRoute.Replace("{reservationId}", reservationId.ToString(), StringComparison.Ordinal);

            var response = await this.Fixture.HttpClient.PutAndDeserializeAsync<ReservationResponse>(reserveSeatsRoute, reserveSeatsContent, this.OutputHelper);

            return response;
        }*/
    }
}