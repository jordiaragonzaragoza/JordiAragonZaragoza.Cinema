namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using Xunit;

    public sealed class SeatNumberTests
    {
        [Fact]
        public void CreateSeatNumber_WhenHavingADefaultValue_ShouldThrowArgumentException()
        {
            // Arrange
            ushort value = default;

            // Act
            Func<SeatNumber> rows = () => SeatNumber.Create(value);

            // Assert
            rows.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateSeatNumber_WhenHavingAValidValue_ShouldReturnSeatNumber()
        {
            // Arrange
            ushort value = 10;

            // Act
            var rows = SeatNumber.Create(value);

            // Assert
            rows.Should().NotBeNull();
        }
    }
}