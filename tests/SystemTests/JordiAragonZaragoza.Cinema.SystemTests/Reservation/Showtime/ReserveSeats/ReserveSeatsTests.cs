namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.ReserveSeats
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.ReserveSeats.Assertions;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class ReserveSeatsTests : BaseSystemTests
    {
        public ReserveSeatsTests(
            SystemTestsFixture fixture,
            ITestOutputHelper output)
            : base(fixture, output)
        {
        }

        [Fact]
        public async Task ReserveSeats_ShouldCreateReservationAndBlockSeats()
        {
            // Arrange
            var scenario = new ReserveSeatsScenario();
            await scenario.ArrangeAsync(this.Fixture.ShowtimeCommandClient, this.Fixture.ShowtimeQueryClient, this.OutputHelper);

            var reservationId = Guid.NewGuid();
            var seatsIds = scenario.AvailableSeatsIds.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).Select(seat => seat.Id).ToList();
            var reserveSeatsRequest = new ReserveSeatsBodyRequest(seatsIds);

            // Act
            await this.Fixture.ShowtimeCommandClient.ReserveSeatsAsync(reservationId, scenario.ShowtimeId, reserveSeatsRequest, this.OutputHelper);

            await EventualConsistency.WaitUntilAsync(
                () => this.Fixture.ShowtimeQueryClient.ShowtimeReservationExistsAsync(reservationId, this.OutputHelper));

            // Assert
            await ReservationAssertions.ShouldExistAsync(
                this.Fixture.ShowtimeQueryClient,
                reservationId,
                scenario.ShowtimeId,
                scenario.ExpectedUserId,
                scenario.SessionDate,
                seatsIds,
                this.OutputHelper);

            await SeatAvailabilityAssertions.ShouldNotContainReservedSeatsAsync(
                this.Fixture.ShowtimeQueryClient,
                scenario.ShowtimeId,
                seatsIds,
                this.OutputHelper);
        }
    }
}