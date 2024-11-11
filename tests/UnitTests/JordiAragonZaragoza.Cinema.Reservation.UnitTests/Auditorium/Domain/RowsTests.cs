namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
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
    }
}