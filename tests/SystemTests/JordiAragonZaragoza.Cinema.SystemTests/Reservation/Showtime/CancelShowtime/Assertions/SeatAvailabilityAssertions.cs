namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.CancelShowtime.Assertions
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit.Abstractions;

    public static class SeatAvailabilityAssertions
    {
        public static async Task ShouldNotBeAvailableAsync(
            ShowtimeQueryClient queryClient,
            Guid showtimeId,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);

            var seats = await queryClient.GetAvailableSeatsAsync(showtimeId, output);

            seats.Should().BeNullOrEmpty();
        }
    }
}