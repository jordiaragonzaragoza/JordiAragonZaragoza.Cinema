namespace JordiAragon.Cinema.Domain.UnitTests.AuditoriumAggregate
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
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
    }
}