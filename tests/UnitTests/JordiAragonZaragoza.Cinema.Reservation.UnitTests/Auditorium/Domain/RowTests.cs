namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using System.Globalization;
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

        [Fact]
        public void ImplicitConversion_WhenHavingARow_ShouldReturnDateTimeOffset()
        {
            // Arrange
            const ushort rowValue = 1;
            var row = Row.Create(rowValue);

            // Act
            ushort result = row;

            // Assert
            result.Should().Be(rowValue);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfRow()
        {
            // Arrange
            const ushort value = 1;
            var row = Row.Create(value);

            // Act
            var result = row.ToString();

            // Assert
            result.Should().Be(value.ToString(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            const ushort value1 = 1;
            const ushort value2 = 2;

            var row1 = Row.Create(value1);
            var row2 = Row.Create(value1);
            var row3 = Row.Create(value2);

            // Act & Assert
            row1.Should().Be(row2);
            row1.Should().NotBe(row3);
        }
    }
}