namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Showtime.Domain
{
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using Xunit;

    public sealed class ReservationTests
    {
        [Fact]
        public void MarkAsPaid_WhenHavingUnPaidReservation_ShouldMarkReservationAsPaid()
        {
            // Arrange
            var reservation = CreateReservationUtils.Create();

            // Act
            reservation.MarkAsPurchased();

            // Assert
            reservation.IsPurchased.Should().Be(true);
        }

        [Fact]
        public void MarkAsPaid_WhenHavingAPaidReservation_ShouldMarkReservationAsPaid()
        {
            // Arrange
            var reservation = CreateReservationUtils.Create();

            reservation.MarkAsPurchased();

            // Act
            reservation.MarkAsPurchased();

            // Assert
            reservation.IsPurchased.Should().Be(true);
        }
    }
}