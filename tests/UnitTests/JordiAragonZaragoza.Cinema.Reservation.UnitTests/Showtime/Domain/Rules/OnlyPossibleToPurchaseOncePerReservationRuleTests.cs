namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Showtime.Domain.Rules
{
    using System;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Rules;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using Xunit;

    public sealed class OnlyPossibleToPurchaseOncePerReservationRuleTests
    {
        [Fact]
        public void ConstructorOnlyPossibleToPayOncePerReservationRule_WhenHavingInvalidArgument_ShouldThrowArgumentException()
        {
            // Arrange
            Reservation reservation = null!;

            // Act
            Func<OnlyPossibleToPurchaseOncePerReservationRule> constructor = () => new OnlyPossibleToPurchaseOncePerReservationRule(reservation);

            // Assert
            constructor.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void IsBroken_WhenReservationIsPurchased_ShouldBeTrue()
        {
            // Arrange
            var reservation = CreateReservationUtils.Create();
            reservation.MarkAsPurchased();

            // Act
            var rule = new OnlyPossibleToPurchaseOncePerReservationRule(reservation);

            // Assert
            rule.IsBroken().Should().Be(true);
        }

        [Fact]
        public void IsBroken_WhenReservationIsNotPaid_ShouldBeFalse()
        {
            // Arrange
            var reservation = CreateReservationUtils.Create();

            // Act
            var rule = new OnlyPossibleToPurchaseOncePerReservationRule(reservation);

            // Assert
            rule.IsBroken().Should().Be(false);
        }
    }
}