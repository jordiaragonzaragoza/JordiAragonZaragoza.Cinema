namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.CancelShowtime
{
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.CancelShowtime.Assertions;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class CancelShowtimeTests : BaseSystemTests
    {
        public CancelShowtimeTests(
            SystemTestsFixture fixture,
            ITestOutputHelper output)
            : base(fixture, output)
        {
        }

        [Fact]
        public async Task CancelShowtime_ShouldRemoveShowtimeAndRelatedData()
        {
            // Arrange
            var scenario = new CancelShowtimeScenario();
            await scenario.ArrangeAsync(this.Fixture.ReservationCommandTestClient, this.Fixture.ReservationQueryTestClient, this.OutputHelper);

            // Act
            await this.Fixture.ReservationCommandTestClient.CancelShowtimeAsync(scenario.ShowtimeId, this.OutputHelper);

            await EventualConsistency.WaitUntilAsync(
                () => this.Fixture.ReservationQueryTestClient.ShowtimeNotExistsAsync(scenario.ShowtimeId, this.OutputHelper));

            // Assert
            await ShowtimeAssertions.ShouldNotExistAsync(
                this.Fixture.ReservationQueryTestClient,
                scenario.ShowtimeId,
                this.OutputHelper);

            await SeatAvailabilityAssertions.ShouldNotBeAvailableAsync(
                this.Fixture.ReservationQueryTestClient,
                scenario.ShowtimeId,
                this.OutputHelper);

            await ShowtimeReservationsAssertions.ShouldNotExistAsync(
                this.Fixture.ReservationQueryTestClient,
                scenario.ShowtimeId,
                this.OutputHelper);
        }
    }
}