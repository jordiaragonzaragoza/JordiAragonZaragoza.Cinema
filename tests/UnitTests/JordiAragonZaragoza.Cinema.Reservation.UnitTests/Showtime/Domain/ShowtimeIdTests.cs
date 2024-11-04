namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Showtime.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using Xunit;

    public sealed class ShowtimeIdTests
    {
        [Fact]
        public void CreateShowtimeId_WhenHavingAnEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<ShowtimeId> showtimeId = () => new ShowtimeId(id);

            // Assert
            showtimeId.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateShowtimeId_WhenHavingAValidGuid_ShouldReturnShowtimeId()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var showtimeId = new ShowtimeId(id);

            // Assert
            showtimeId.Should().NotBeNull();
        }
    }
}