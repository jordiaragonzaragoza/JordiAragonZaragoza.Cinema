namespace JordiAragon.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using Xunit;

    public sealed class RowsTests
    {
        [Fact]
        public void CreateRows_WhenHavingADefaultValue_ShouldThrowArgumentException()
        {
            // Arrange
            ushort value = default;

            // Act
            Func<Rows> rows = () => Rows.Create(value);

            // Assert
            rows.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateRows_WhenHavingAValidValue_ShouldReturnRows()
        {
            // Arrange
            ushort value = 10;

            // Act
            var rows = Rows.Create(value);

            // Assert
            rows.Should().NotBeNull();
        }
    }
}