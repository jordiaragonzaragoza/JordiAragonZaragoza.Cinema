namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.CancelShowtime.Assertions
{
    using System;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using Xunit.Abstractions;

    public static class ShowtimeAssertions
    {
        public static async Task ShouldNotExistAsync(
            ShowtimeQueryClient queryClient,
            Guid showtimeId,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);

            var showtime = await queryClient.GetShowtimeAsync(showtimeId, output);

            showtime.Should().BeNull();
        }
    }
}