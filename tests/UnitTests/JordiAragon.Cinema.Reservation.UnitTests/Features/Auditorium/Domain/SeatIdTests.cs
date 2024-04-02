namespace JordiAragon.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using Xunit;

    public sealed class SeatIdTests
    {
        [Fact]
        public void CreateSeatId_WhenHavingAnEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<SeatId> seatId = () => SeatId.Create(id);

            // Assert
            seatId.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateSeatId_WhenHavingAValidGuid_ShouldReturnSeatId()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var seatId = SeatId.Create(id);

            // Assert
            seatId.Should().NotBeNull();
        }
    }
}