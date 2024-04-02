namespace JordiAragon.Cinema.Reservation.UnitTests.Showtime.Domain.Rules
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Rules;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using Xunit;

    public sealed class OnlyPossibleToPurchaseOncePerTicketRuleTests
    {
        [Fact]
        public void ConstructorOnlyPossibleToPayOncePerTicketRule_WhenHavingInvalidArgument_ShouldThrowArgumentException()
        {
            // Arrange
            Ticket reservation = null;

            // Act
            Func<OnlyPossibleToPurchaseOncePerTicketRule> constructor = () => new OnlyPossibleToPurchaseOncePerTicketRule(reservation);

            // Assert
            constructor.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void IsBroken_WhenReservationIsPurchased_ShouldBeTrue()
        {
            // Arrange
            var ticket = CreateTicketUtils.Create();
            ticket.MarkAsPurchased();

            // Act
            var rule = new OnlyPossibleToPurchaseOncePerTicketRule(ticket);

            // Assert
            rule.IsBroken().Should().Be(true);
        }

        [Fact]
        public void IsBroken_WhenReservationIsNotPaid_ShouldBeFalse()
        {
            // Arrange
            var ticket = CreateTicketUtils.Create();

            // Act
            var rule = new OnlyPossibleToPurchaseOncePerTicketRule(ticket);

            // Assert
            rule.IsBroken().Should().Be(false);
        }
    }
}