namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using Xunit;

    public sealed class SeatIdTests
    {
        [Fact]
        public void CreateSeatId_WhenHavingAnEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<SeatId> seatId = () => new SeatId(id);

            // Assert
            seatId.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ImplicitConversion_WhenHavingASeatId_ShouldReturnGuid()
        {
            // Arrange
            var value = Guid.NewGuid();
            var seatId = new SeatId(value);

            // Act
            Guid result = seatId;

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfSeatId()
        {
            // Arrange
            var value = Guid.NewGuid();
            var seatId = new SeatId(value);

            // Act
            var result = seatId.ToString();

            // Assert
            result.Should().Be(value.ToString());
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            var value1 = Guid.NewGuid();
            var value2 = Guid.NewGuid();

            var seatId1 = new SeatId(value1);
            var seatId2 = new SeatId(value1);
            var seatId3 = new SeatId(value2);

            // Act & Assert
            seatId1.Should().Be(seatId2);
            seatId1.Should().NotBe(seatId3);
        }
    }
}