namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.CancelShowtime.Assertions
{
    using System;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using Xunit.Abstractions;

    public static class SeatAvailabilityAssertions
    {
        public static async Task ShouldNotBeAvailableAsync(
            ReservationQueryTestClient queryClient,
            Guid showtimeId,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);

            var seats = await queryClient.GetAvailableSeatsAsync(showtimeId, output);

            seats.Should().BeNullOrEmpty();
        }
    }
}