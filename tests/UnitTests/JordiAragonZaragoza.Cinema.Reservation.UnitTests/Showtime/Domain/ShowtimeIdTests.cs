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
        public void ImplicitConversion_WhenHavingAShowtimeId_ShouldReturnGuid()
        {
            // Arrange
            var value = Guid.NewGuid();
            var showtimeId = new ShowtimeId(value);

            // Act
            Guid result = showtimeId;

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfShowtimeId()
        {
            // Arrange
            var value = Guid.NewGuid();
            var showtimeId = new ShowtimeId(value);

            // Act
            var result = showtimeId.ToString();

            // Assert
            result.Should().Be(value.ToString());
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            var value1 = Guid.NewGuid();
            var value2 = Guid.NewGuid();

            var showtimeId1 = new ShowtimeId(value1);
            var showtimeId2 = new ShowtimeId(value1);
            var showtimeId3 = new ShowtimeId(value2);

            // Act & Assert
            showtimeId1.Should().Be(showtimeId2);
            showtimeId1.Should().NotBe(showtimeId3);
        }
    }
}