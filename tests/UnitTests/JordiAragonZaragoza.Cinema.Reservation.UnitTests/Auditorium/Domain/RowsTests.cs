namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using System.Globalization;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;
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
            rows.Should().Throw<BusinessRuleValidationException>().WithMessage("The minimum rows value must be valid.");
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

        [Fact]
        public void ImplicitConversion_WhenHavingARows_ShouldReturnDateTimeOffset()
        {
            // Arrange
            const ushort rowsValue = 1;
            var rows = Rows.Create(rowsValue);

            // Act
            ushort result = rows;

            // Assert
            result.Should().Be(rowsValue);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfRows()
        {
            // Arrange
            const ushort value = 1;
            var rows = Rows.Create(value);

            // Act
            var result = rows.ToString();

            // Assert
            result.Should().Be(value.ToString(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            const ushort value1 = 1;
            const ushort value2 = 2;

            var rows1 = Rows.Create(value1);
            var rows2 = Rows.Create(value1);
            var rows3 = Rows.Create(value2);

            // Act & Assert
            rows1.Should().Be(rows2);
            rows1.Should().NotBe(rows3);
        }
    }
}