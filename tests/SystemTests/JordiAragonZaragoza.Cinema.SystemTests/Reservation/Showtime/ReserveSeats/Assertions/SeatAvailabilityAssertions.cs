namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.ReserveSeats.Assertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit.Abstractions;

    public static class SeatAvailabilityAssertions
    {
        public static async Task ShouldNotContainReservedSeatsAsync(
            ShowtimeQueryClient queryClient,
            Guid showtimeId,
            IEnumerable<Guid> reservedSeatsIds,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);

            var seats = await queryClient.GetAvailableSeatsAsync(showtimeId, output);

            seats.Should().NotBeNullOrEmpty();

            seats.Select(seatResponse => seatResponse.Id).Should()
                .NotContain(reservedSeatsIds);
        }
    }
}