namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.CancelReservation
{
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.CancelReservation.Assertions;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class CancelReservationTests : BaseSystemTests
    {
        public CancelReservationTests(
            SystemTestsFixture fixture,
            ITestOutputHelper output)
            : base(fixture, output)
        {
        }

        [Fact]
        public async Task CancelExistingReservation_WhenHavingValidArguments_ShouldMarkReservationAsCancelled()
        {
            // Arrange
            var scenario = new CancelReservationScenario();
            await scenario.ArrangeAsync(this.Fixture.ReservationCommandTestClient, this.Fixture.ReservationQueryTestClient, this.OutputHelper);

            // Act
            await this.Fixture.ReservationCommandTestClient.CancelReservationAsync(scenario.ShowtimeId, scenario.ReservationId, this.OutputHelper);

            await EventualConsistency.WaitUntilAsync(
                () => this.Fixture.ReservationQueryTestClient.ShowtimeReservationNotExistsAsync(scenario.ReservationId, this.OutputHelper));

            // Assert
            await ReservationAssertions.ShouldBeCancelledAsync(
                this.Fixture.ReservationQueryTestClient,
                scenario.ReservationId,
                this.OutputHelper);
        }
    }
}