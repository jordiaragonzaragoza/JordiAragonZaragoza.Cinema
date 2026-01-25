namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.V2.Showtime
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.Common;
    using Xunit;
    using Xunit.Abstractions;

    using SeedData = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedData;

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
            var sessionDateOnUtc = SeedData.ExampleShowtime.SessionDateOnUtc;

            var showtimeId = SeedData.ExampleShowtime.Id;

            var seatsIds = SeedData.ExampleAvailableSeatsReadModel.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).Select(seat => seat.SeatId).ToList();

            var reservationId = Guid.NewGuid();
            var reserveSeatsRequest = new ReserveSeatsBodyRequest(seatsIds);
            var reserveSeatsContent = StringContentHelpers.FromModelAsJson(reserveSeatsRequest);

            var reserveSeatsRoute = $"{Routes.ApiBase}{ShowtimeRoutes.ReserveSeats}";
            reserveSeatsRoute = reserveSeatsRoute.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);
            reserveSeatsRoute = reserveSeatsRoute.Replace("{reservationId}", reservationId.ToString(), StringComparison.Ordinal);

            // Act
            var reservationResponse = await this.Fixture.HttpClient.PutAndDeserializeAsync<ReservationResponse>(reserveSeatsRoute, reserveSeatsContent, this.OutputHelper);

            // Assert
            reservationResponse.SessionDateOnUtc.Should()
                .Be(sessionDateOnUtc);

            reservationResponse.AuditoriumName.Should()
                .Be(SeedData.ExampleAuditorium.Name);

            reservationResponse.MovieTitle.Should()
                .Be(SeedData.ExampleMovie.Title);

            reservationResponse.Seats.Select(seatResponse => seatResponse.Id).Should()
                .Contain(seatsIds);

            reservationResponse.IsPurchased.Should().BeFalse();
        }
    }
}