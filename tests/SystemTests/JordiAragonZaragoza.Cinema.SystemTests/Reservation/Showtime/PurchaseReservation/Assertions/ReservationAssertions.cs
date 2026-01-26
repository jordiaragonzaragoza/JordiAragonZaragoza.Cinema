namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime.PurchaseReservation.Assertions
{
    using System;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using Xunit.Abstractions;

    public static class ReservationAssertions
    {
        public static async Task ShouldBePurchasedAsync(
            ShowtimeQueryClient queryClient,
            Guid reservationId,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(queryClient);

            var reservation = await queryClient.GetShowtimeReservationAsync(reservationId, output);

            reservation.Should().NotBeNull();
            reservation.Id.Should().Be(reservationId, "ReservationId should match the reservation");
            reservation.IsPurchased.Should().BeTrue("Reservation should be purchased");
        }
    }
}