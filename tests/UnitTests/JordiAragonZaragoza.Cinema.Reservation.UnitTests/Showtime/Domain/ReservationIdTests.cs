namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Showtime.Domain
{
    using System;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using Xunit;

    public sealed class ReservationIdTests
    {
        [Fact]
        public void CreateReservationId_WhenHavingAnEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<ReservationId> reservationId = () => new ReservationId(id);

            // Assert
            reservationId.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ImplicitConversion_WhenHavingAReservationId_ShouldReturnGuid()
        {
            // Arrange
            var value = Guid.CreateVersion7();
            var reservationId = new ReservationId(value);

            // Act
            Guid result = reservationId;

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfReservationId()
        {
            // Arrange
            var value = Guid.CreateVersion7();
            var reservationId = new ReservationId(value);

            // Act
            var result = reservationId.ToString();

            // Assert
            result.Should().Be(value.ToString());
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            var value1 = Guid.CreateVersion7();
            var value2 = Guid.CreateVersion7();

            var reservationId1 = new ReservationId(value1);
            var reservationId2 = new ReservationId(value1);
            var reservationId3 = new ReservationId(value2);

            // Act & Assert
            reservationId1.Should().Be(reservationId2);
            reservationId1.Should().NotBe(reservationId3);
        }
    }
}