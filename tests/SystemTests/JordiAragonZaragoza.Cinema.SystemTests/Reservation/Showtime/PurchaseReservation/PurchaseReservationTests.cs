namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.PurchaseReservation
{
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.PurchaseReservation.Assertions;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class PurchaseReservationTests : BaseSystemTests
    {
        public PurchaseReservationTests(
            SystemTestsFixture fixture,
            ITestOutputHelper output)
            : base(fixture, output)
        {
        }

        [Fact]
        public async Task PurchaseExistingReservation_WhenHavingValidArguments_ShouldMarkReservationAsPaid()
        {
            // Arrange
            var scenario = new PurchaseReservationScenario();
            await scenario.ArrangeAsync(this.Fixture.ShowtimeCommandClient, this.Fixture.ShowtimeQueryClient, this.OutputHelper);

            // Act
            await this.Fixture.ShowtimeCommandClient.PurchaseReservationAsync(scenario.ShowtimeId, scenario.ReservationId, this.OutputHelper);

            await EventualConsistency.WaitUntilAsync(
                () => this.Fixture.ShowtimeQueryClient.PurchasedReservationExistsAsync(scenario.ReservationId, this.OutputHelper));

            // Assert
            await ReservationAssertions.ShouldBePurchasedAsync(
                this.Fixture.ShowtimeQueryClient,
                scenario.ReservationId,
                this.OutputHelper);
        }
    }
}