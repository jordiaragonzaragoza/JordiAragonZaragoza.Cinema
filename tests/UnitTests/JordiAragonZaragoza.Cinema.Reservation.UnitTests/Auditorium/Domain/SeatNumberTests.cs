namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;
    using Xunit;

    public sealed class SeatNumberTests
    {
        [Fact]
        public void CreateSeatNumber_WhenHavingADefaultValue_ShouldThrowArgumentException()
        {
            // Arrange
            ushort value = default;

            // Act
            Func<SeatNumber> rows = () => SeatNumber.Create(value);

            // Assert
            rows.Should().Throw<BusinessRuleValidationException>().WithMessage("The minimum seat number value must be valid.");
        }

        [Fact]
        public void CreateSeatNumber_WhenHavingAValidValue_ShouldReturnSeatNumber()
        {
            // Arrange
            ushort value = 10;

            // Act
            var rows = SeatNumber.Create(value);

            // Assert
            rows.Should().NotBeNull();
        }
    }
}