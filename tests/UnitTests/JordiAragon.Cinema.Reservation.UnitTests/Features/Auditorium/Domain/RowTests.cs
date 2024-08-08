namespace JordiAragon.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using Xunit;

    public sealed class RowTests
    {
        [Fact]
        public void CreateRow_WhenHavingADefaultValue_ShouldThrowArgumentException()
        {
            // Arrange
            ushort value = default;

            // Act
            Func<Row> row = () => Row.Create(value);

            // Assert
            row.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateRow_WhenHavingAValidValue_ShouldReturnRow()
        {
            // Arrange
            ushort value = 10;

            // Act
            var row = Row.Create(value);

            // Assert
            row.Should().NotBeNull();
        }
    }
}