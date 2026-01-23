namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.CancelShowtime.Assertions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit.Abstractions;

    public static class ShowtimeReservationsAssertions
    {
        public static async Task ShouldNotExistAsync(
            ShowtimeQueryClient queryClient,
            Guid showtimeId,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);

            var reservations = await queryClient.GetShowtimeReservationsAsync(showtimeId, output);
            (reservations?.Items.Count() ?? 0).Should().Be(0);
        }
    }
}