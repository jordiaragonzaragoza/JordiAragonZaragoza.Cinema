namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
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
            ushort value = 10;

            // Act
            var rows = SeatsPerRow.Create(value);

            // Assert
            rows.Should().NotBeNull();
        }
    }
}