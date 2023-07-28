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
    }
}