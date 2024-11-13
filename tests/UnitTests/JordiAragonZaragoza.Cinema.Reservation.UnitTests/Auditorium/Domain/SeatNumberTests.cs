namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using System.Globalization;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;
    using Xunit;

    public sealed class SeatNumberTests
    {
        [Fact]
        public void CreateSeatNumber_WhenHavingADefaultValue_ShouldThseatNumberArgumentException()
        {
            // Arrange
            ushort value = default;

            // Act
            Func<SeatNumber> seatNumbers = () => SeatNumber.Create(value);

            // Assert
            seatNumbers.Should().Throw<BusinessRuleValidationException>().WithMessage("The minimum seat number value must be valid.");
        }

        [Fact]
        public void CreateSeatNumber_WhenHavingAValidValue_ShouldReturnSeatNumber()
        {
            // Arrange
            ushort value = 10;

            // Act
            var seatNumbers = SeatNumber.Create(value);

            // Assert
            seatNumbers.Should().NotBeNull();
        }

        [Fact]
        public void ImplicitConversion_WhenHavingASeatNumber_ShouldReturnDateTimeOffset()
        {
            // Arrange
            const ushort seatNumberValue = 1;
            var seatNumber = SeatNumber.Create(seatNumberValue);

            // Act
            ushort result = seatNumber;

            // Assert
            result.Should().Be(seatNumberValue);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfSeatNumber()
        {
            // Arrange
            const ushort value = 1;
            var seatNumber = SeatNumber.Create(value);

            // Act
            var result = seatNumber.ToString();

            // Assert
            result.Should().Be(value.ToString(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            const ushort value1 = 1;
            const ushort value2 = 2;

            var seatNumber1 = SeatNumber.Create(value1);
            var seatNumber2 = SeatNumber.Create(value1);
            var seatNumber3 = SeatNumber.Create(value2);

            // Act & Assert
            seatNumber1.Should().Be(seatNumber2);
            seatNumber1.Should().NotBe(seatNumber3);
        }
    }
}