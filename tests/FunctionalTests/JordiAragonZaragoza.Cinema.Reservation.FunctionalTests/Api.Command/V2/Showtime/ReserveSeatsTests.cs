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

    using SeedValues = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedValues;

    public sealed class ReserveSeatsTests : BaseHttpRestfulApiFunctionalTests
    {
        public ReserveSeatsTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task CreateReservationOnExistingShowtime_WhenHavingValidArguments_ShouldCreateRequiredReservation()
        {
            // Arrange
            var sessionDateOnUtc = this.Fixture.SeedData.ExampleShowtime.SessionDateOnUtc;

            var showtimeId = SeedValues.ExampleShowtimeId;

            var seatsIds = this.Fixture.SeedData.ExampleAvailableSeatsReadModel.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).Select(seat => seat.SeatId).ToList();

            var reservationId = Guid.CreateVersion7();
            var reserveSeatsRequest = new ReserveSeatsBodyRequest(seatsIds);
            var reserveSeatsContent = StringContentHelpers.FromModelAsJson(reserveSeatsRequest);

            var reserveSeatsRoute = $"{Routes.ApiBase}{ShowtimeRoutes.ReserveSeats}";
            reserveSeatsRoute = reserveSeatsRoute.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);
            reserveSeatsRoute = reserveSeatsRoute.Replace("{reservationId}", reservationId.ToString(), StringComparison.Ordinal);

            // Act
            var reservationResponse = await this.Fixture.HttpClient.PutAndDeserializeAsync<ReservationResponse>(reserveSeatsRoute, reserveSeatsContent, this.OutputHelper);

            // Assert
            reservationResponse.SessionDateOnUtc.Should()
                .BeCloseTo(sessionDateOnUtc, TimeSpan.FromMilliseconds(1));

            reservationResponse.AuditoriumName.Should()
                .Be(this.Fixture.SeedData.ExampleAuditorium.Name);

            reservationResponse.MovieTitle.Should()
                .Be(this.Fixture.SeedData.ExampleMovie.Title);

            reservationResponse.Seats.Select(seatResponse => seatResponse.Id).Should()
                .Contain(seatsIds);

            reservationResponse.IsPurchased.Should().BeFalse();
        }
    }
}