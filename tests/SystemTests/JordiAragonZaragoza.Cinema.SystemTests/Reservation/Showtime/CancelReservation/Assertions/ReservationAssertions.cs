namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.CancelReservation.Assertions
{
    using System;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using Xunit.Abstractions;

    public static class ReservationAssertions
    {
        public static async Task ShouldBeCancelledAsync(
            ReservationQueryTestClient queryClient,
            Guid reservationId,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);

            var reservation = await queryClient.GetShowtimeReservationAsync(reservationId, output);

            reservation.Should().BeNull();
        }
    }
}