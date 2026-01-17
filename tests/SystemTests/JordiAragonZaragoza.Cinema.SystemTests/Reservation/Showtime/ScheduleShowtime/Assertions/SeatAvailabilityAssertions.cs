namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.ScheduleShowtime.Assertions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;

    public static class SeatAvailabilityAssertions
    {
        public static async Task ShouldBeAvailableAsync(
            ShowtimeQueryClient queryClient,
            Guid showtimeId)
        {
            ArgumentNullException.ThrowIfNull(queryClient);

            var seats = await queryClient.GetAvailableSeatsAsync(showtimeId);

            seats.Should().NotBeNullOrEmpty();
            seats.Count().Should().Be(
                SeedData.ExampleAuditorium.Rows *
                SeedData.ExampleAuditorium.SeatsPerRow);
        }
    }
}