namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.Showtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2;
    using JordiAragonZaragoza.Cinema.Reservation.User.Presentation.HttpRestfulApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    using Constants = JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain.Constants;

    public sealed class PurchaseReservationTests : BaseHttpRestfulApiFunctionalTests
    {
        public PurchaseReservationTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task PurchaseReservation_WhenHavingValidArguments_ShouldMarkReservationAsPaid()
        {
            // Arrange
            var sessionDateOnUtc = DateTimeOffset.UtcNow.AddDays(1);

            var showtimeId = await this.ScheduleNewShowtimeAsync(sessionDateOnUtc);

            var routeAvailableSeats = $"api/v2/{GetAvailableSeats.Route}";
            routeAvailableSeats = routeAvailableSeats.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(routeAvailableSeats, this.OutputHelper);

            var seatsIds = availableSeatsResponse.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).Select(seat => seat.Id);

            var reserveSeatsRequest = new ReserveSeatsRequest(showtimeId, seatsIds);
            var reserveSeatsContent = StringContentHelpers.FromModelAsJson(reserveSeatsRequest);

            var routeReserveSeats = $"api/v2/{ReserveSeats.Route}";
            routeReserveSeats = routeReserveSeats.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            var reservationReserveResponse = await this.Fixture.HttpClient.PostAndDeserializeAsync<ReservationResponse>(routeReserveSeats, reserveSeatsContent, this.OutputHelper);
            await AddEventualConsistencyDelayAsync();

            var reservationId = reservationReserveResponse.Id;

            var routePurchaseReservation = $"api/v2/{PurchaseReservation.Route}";
            routePurchaseReservation = routePurchaseReservation.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);
            routePurchaseReservation = routePurchaseReservation.Replace("{reservationId}", reservationId.ToString(), StringComparison.Ordinal);

            var fullUriPurchaseReservation = new Uri(this.Fixture.HttpClient.BaseAddress!, routePurchaseReservation);

            var purchaseReservationRequest = new PurchaseReservationRequest(showtimeId, reservationReserveResponse.Id, true);
            var purchaseReservationContent = StringContentHelpers.FromModelAsJson(purchaseReservationRequest);

            // Act
            this.OutputHelper.WriteLine($"Requesting with PATCH {routePurchaseReservation}");
            var response = await this.Fixture.HttpClient.PatchAsync(fullUriPurchaseReservation, purchaseReservationContent);
            await AddEventualConsistencyDelayAsync();

            // Assert
            response.StatusCode.Should()
               .Be(HttpStatusCode.NoContent);

            await this.TestProjectionsAsync(showtimeId, seatsIds, reservationId, sessionDateOnUtc);
        }

        private async Task TestProjectionsAsync(Guid showtimeId, IEnumerable<Guid> seatsIds, Guid reservationId, DateTimeOffset sessionDateOnUtc)
        {
            await this.GetUserReservation_WhenReservationPurchased_ShouldReturnReservationPurchased(showtimeId, seatsIds, reservationId, sessionDateOnUtc);
        }

        private async Task GetUserReservation_WhenReservationPurchased_ShouldReturnReservationPurchased(Guid showtimeId, IEnumerable<Guid> seatsIds, Guid reservationId, DateTimeOffset sessionDateOnUtc)
        {
            var userId = SeedData.ExampleUser.Id;
            var routeUserReservation = $"api/v2/{GetUserReservation.Route}";
            routeUserReservation = routeUserReservation.Replace("{userId}", userId.ToString(), StringComparison.Ordinal);
            routeUserReservation = routeUserReservation.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);
            routeUserReservation = routeUserReservation.Replace("{reservationId}", reservationId.ToString(), StringComparison.Ordinal);

            var reservationPurchasedResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<ReservationResponse>(routeUserReservation, this.OutputHelper);

            reservationPurchasedResponse.SessionDateOnUtc.Should()
                .Be(sessionDateOnUtc);

            reservationPurchasedResponse.AuditoriumName.Should()
                .Be(SeedData.ExampleAuditorium.Name);

            reservationPurchasedResponse.MovieTitle.Should()
                .Be(SeedData.ExampleMovie.Title);

            reservationPurchasedResponse.Seats.Select(seatResponse => seatResponse.Id).Should()
                .Contain(seatsIds);

            reservationPurchasedResponse.IsPurchased.Should().BeTrue();
        }

        private async Task<Guid> ScheduleNewShowtimeAsync(DateTimeOffset sessionDateOnUtc)
        {
            var url = $"api/v2/{ScheduleShowtime.Route}";

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