namespace JordiAragon.Cinema.Domain.UnitTests.ShowtimeAggregate
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using Xunit;

    public class ShowtimeIdTests
    {
        [Fact]
        public void CreateShowtimeId_WhenHavingAnEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<ShowtimeId> showtimeId = () => ShowtimeId.Create(id);

            // Assert
            showtimeId.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateShowtimeId_WhenHavingAValidGuid_ShouldReturnShowtimeId()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var showtimeId = ShowtimeId.Create(id);

            // Assert
            showtimeId.Should().NotBeNull();
        }
    }
}