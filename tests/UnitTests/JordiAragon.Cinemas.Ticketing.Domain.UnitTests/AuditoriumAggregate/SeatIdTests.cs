namespace JordiAragon.Cinemas.Ticketing.Domain.UnitTests.AuditoriumAggregate
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate;
    using Xunit;

    public class SeatIdTests
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