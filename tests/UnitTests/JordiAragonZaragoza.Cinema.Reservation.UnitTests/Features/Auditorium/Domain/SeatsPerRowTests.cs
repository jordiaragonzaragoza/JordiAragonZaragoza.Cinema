namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using Xunit;

    public sealed class SeatsPerRowTests
    {
        [Fact]
        public void CreateSeatsPerRow_WhenHavingADefaultValue_ShouldThrowArgumentException()
        {
            // Arrange
            ushort value = default;

            // Act
            Func<SeatsPerRow> rows = () => SeatsPerRow.Create(value);

            // Assert
            rows.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateSeatsPerRow_WhenHavingAValidValue_ShouldReturnSeatsPerRow()
        {
            // Arrange
            ushort value = 10;

            // Act
            var rows = SeatsPerRow.Create(value);

            // Assert
            rows.Should().NotBeNull();
        }
    }
}