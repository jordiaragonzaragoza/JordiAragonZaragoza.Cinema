namespace JordiAragon.Cinemas.Ticketing.UnitTests.Showtime.Domain.Rules
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain.Rules;
    using JordiAragon.Cinemas.Ticketing.UnitTests.TestUtils.Domain;
    using Xunit;

    public class OnlyPossibleToPurchaseOncePerTicketRuleTests
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
        public void IsBroken_WhenReservationIsPaid_ShouldBeTrue()
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