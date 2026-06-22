namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.V2.Showtime
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.Common;
    using Xunit;
    using Xunit.Abstractions;

    using SeedData = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedData;

    public sealed class CancelReservationTests : BaseHttpRestfulApiFunctionalTests
    {
        public CancelReservationTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task CancelReservation_WhenHavingValidArguments_ShouldCancelReservation()
        {
            // Arrange
            var showtimeId = SeedData.ExampleShowtime.Id;

            var seatsIds = SeedData.ExampleAvailableSeatsReadModel.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).Select(seat => seat.SeatId).ToList();

            var reservationId = Guid.CreateVersion7();
            var reserveSeatsRequest = new ReserveSeatsBodyRequest(seatsIds);
            var reserveSeatsContent = StringContentHelpers.FromModelAsJson(reserveSeatsRequest);

            var reserveSeatsRoute = $"{Routes.ApiBase}{ShowtimeRoutes.ReserveSeats}";
            reserveSeatsRoute = reserveSeatsRoute.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);
            reserveSeatsRoute = reserveSeatsRoute.Replace("{reservationId}", reservationId.ToString(), StringComparison.Ordinal);

            _ = await this.Fixture.HttpClient.PutAndDeserializeAsync<ReservationResponse>(reserveSeatsRoute, reserveSeatsContent, this.OutputHelper);

            // Act
            var cancelReservationRoute = $"{Routes.ApiBase}{ShowtimeRoutes.CancelReservation}";
            cancelReservationRoute = cancelReservationRoute.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);
            cancelReservationRoute = cancelReservationRoute.Replace("{reservationId}", reservationId.ToString(), StringComparison.Ordinal);

            var fullCancelReservationUri = new Uri(this.Fixture.HttpClient.BaseAddress!, cancelReservationRoute);

            // Assert
            this.OutputHelper.WriteLine($"Requesting with DELETE {cancelReservationRoute}");
            var cancelReservationResponse = await this.Fixture.HttpClient.DeleteAsync(fullCancelReservationUri);

            cancelReservationResponse.StatusCode.Should()
                .Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}