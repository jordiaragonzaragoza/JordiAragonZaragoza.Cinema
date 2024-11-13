namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using System.Globalization;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;
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
            rows.Should().Throw<BusinessRuleValidationException>().WithMessage("The minimum seat per row value must be valid.");
        }

        [Fact]
        public void CreateSeatsPerRow_WhenHavingAValidValue_ShouldReturnSeatsPerRow()
        {
            // Arrange
            const ushort value = 10;

            // Act
            var rows = SeatsPerRow.Create(value);

            // Assert
            rows.Should().NotBeNull();
        }

        [Fact]
        public void ImplicitConversion_WhenHavingASeatsPerRow_ShouldReturnDateTimeOffset()
        {
            // Arrange
            const ushort seatsPerRowValue = 1;
            var seatsPerRow = SeatsPerRow.Create(seatsPerRowValue);

            // Act
            ushort result = seatsPerRow;

            // Assert
            result.Should().Be(seatsPerRowValue);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfSeatsPerRow()
        {
            // Arrange
            const ushort value = 1;
            var seatsPerRow = SeatsPerRow.Create(value);

            // Act
            var result = seatsPerRow.ToString();

            // Assert
            result.Should().Be(value.ToString(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            const ushort value1 = 1;
            const ushort value2 = 2;

            var seatsPerRow1 = SeatsPerRow.Create(value1);
            var seatsPerRow2 = SeatsPerRow.Create(value1);
            var seatsPerRow3 = SeatsPerRow.Create(value2);

            // Act & Assert
            seatsPerRow1.Should().Be(seatsPerRow2);
            seatsPerRow1.Should().NotBe(seatsPerRow3);
        }
    }
}