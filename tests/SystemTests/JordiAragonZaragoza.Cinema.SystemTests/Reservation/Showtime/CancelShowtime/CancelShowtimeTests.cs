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
            await scenario.ArrangeAsync(this.Fixture.ShowtimeCommandClient, this.Fixture.ShowtimeQueryClient, this.OutputHelper);

            // Act
            await this.Fixture.ShowtimeCommandClient.CancelShowtimeAsync(scenario.ShowtimeId, this.OutputHelper);

            await EventualConsistency.WaitUntilAsync(
                () => this.Fixture.ShowtimeQueryClient.ShowtimeNotExistsAsync(scenario.ShowtimeId, this.OutputHelper));

            // Assert
            await ShowtimeAssertions.ShouldNotExistAsync(
                this.Fixture.ShowtimeQueryClient,
                scenario.ShowtimeId,
                this.OutputHelper);

            await SeatAvailabilityAssertions.ShouldNotBeAvailableAsync(
                this.Fixture.ShowtimeQueryClient,
                scenario.ShowtimeId,
                this.OutputHelper);

            await ShowtimeReservationsAssertions.ShouldNotExistAsync(
                this.Fixture.ShowtimeQueryClient,
                scenario.ShowtimeId,
                this.OutputHelper);
        }
    }
}