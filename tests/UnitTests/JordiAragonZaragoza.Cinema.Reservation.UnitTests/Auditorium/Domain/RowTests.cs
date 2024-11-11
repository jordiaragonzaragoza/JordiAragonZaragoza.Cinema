namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
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
            row.Should().Throw<BusinessRuleValidationException>().WithMessage("The minimum row value must be valid.");
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