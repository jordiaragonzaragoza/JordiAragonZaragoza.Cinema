namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.CancelShowtime.Assertions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    using Xunit.Abstractions;

    public static class ShowtimeReservationsAssertions
    {
        public static async Task ShouldNotExistAsync(
            ReservationQueryTestClient queryClient,
            Guid showtimeId,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);

            var paginatedRequest = new PaginatedRequest
            {
                PageNumber = 1,
                PageSize = 10,
            };

            var reservations = await queryClient.GetShowtimeReservationsAsync(showtimeId, paginatedRequest, output);
            (reservations?.Items.Count() ?? 0).Should().Be(0);
        }
    }
}