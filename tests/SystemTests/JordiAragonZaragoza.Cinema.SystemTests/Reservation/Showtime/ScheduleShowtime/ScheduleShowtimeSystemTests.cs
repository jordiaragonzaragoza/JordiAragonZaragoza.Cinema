namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.ScheduleShowtime
{
    using System;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.ScheduleShowtime.Assertions;
    using Xunit;
    using Xunit.Abstractions;

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
            var showtimeId = Guid.NewGuid();
            var sessionDate = DateTimeOffset.UtcNow.AddDays(1);

            var request = new ScheduleShowtimeBodyRequest(
                SeedData.ExampleAuditorium.Id,
                SeedData.ExampleMovie.Id,
                sessionDate);

            // Act
            await this.Fixture.ShowtimeCommandClient.ScheduleShowtimeAsync(showtimeId, request);

            await EventualConsistency.WaitUntilAsync(
                () => this.Fixture.ShowtimeQueryClient.ShowtimeExistsAsync(showtimeId));

            // Assert
            await ShowtimeAssertions.ShouldExistAsync(
                this.Fixture.ShowtimeQueryClient,
                showtimeId,
                sessionDate);

            await SeatAvailabilityAssertions.ShouldBeAvailableAsync(
                this.Fixture.ShowtimeQueryClient,
                showtimeId);
        }
    }
}