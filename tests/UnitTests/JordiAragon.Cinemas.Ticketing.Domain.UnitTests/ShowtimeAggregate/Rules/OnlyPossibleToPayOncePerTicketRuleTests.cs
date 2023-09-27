namespace JordiAragon.Cinemas.Ticketing.Domain.UnitTests.ShowtimeAggregate.Rules
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate;
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate.Rules;
    using JordiAragon.Cinemas.Ticketing.Domain.UnitTests.TicketAggregate.TestUtils;
    using Xunit;

    public class OnlyPossibleToPayOncePerTicketRuleTests
    {
        [Fact]
        public void ConstructorOnlyPossibleToPayOncePerTicketRule_WhenHavingInvalidArgument_ShouldThrowArgumentException()
        {
            // Arrange
            Ticket reservation = null;

            // Act
            Func<OnlyPossibleToPayOncePerTicketRule> constructor = () => new OnlyPossibleToPayOncePerTicketRule(reservation);

            // Assert
            constructor.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void IsBroken_WhenReservationIsPaid_ShouldBeTrue()
        {
            // Arrange
            var ticket = CreateTicketUtils.Create();
            ticket.MarkAsPaid();

            // Act
            var rule = new OnlyPossibleToPayOncePerTicketRule(ticket);

            // Assert
            rule.IsBroken().Should().Be(true);
        }

        [Fact]
        public void IsBroken_WhenReservationIsNotPaid_ShouldBeFalse()
        {
            // Arrange
            var ticket = CreateTicketUtils.Create();

            // Act
            var rule = new OnlyPossibleToPayOncePerTicketRule(ticket);

            // Assert
            rule.IsBroken().Should().Be(false);
        }
    }
}