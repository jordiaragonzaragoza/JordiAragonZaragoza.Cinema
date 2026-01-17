namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime
{
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using Xunit.Abstractions;
    using Xunit;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.Assertions;

    public sealed class ScheduleShowtimeSystemTests : BaseSystemTests
    {
        public ScheduleShowtimeSystemTests(
            SystemTestsFixture fixture,
            ITestOutputHelper output)
            : base(fixture, output)
        {
        }

        [Fact]
        public async Task Schedule_showtime_creates_showtime_and_seats()
        {
            // Arrange
            var scenario = new ScheduleShowtimeScenario();

            // Act
            await scenario.ExecuteAsync(this.Fixture.ReservationCommandClient);

            await EventualConsistency.WaitUntilAsync(
                () => this.Fixture.ReservationQueryClient.ShowtimeExistsAsync(scenario.ShowtimeId));

            // Assert
            await ShowtimeAssertions.ShouldExistAsync(
                this.Fixture.ReservationQueryClient,
                scenario.ShowtimeId,
                scenario.SessionDate);

            await SeatAvailabilityAssertions.ShouldBeAvailableAsync(
                this.Fixture.ReservationQueryClient,
                scenario.ShowtimeId);
        }
    }
}