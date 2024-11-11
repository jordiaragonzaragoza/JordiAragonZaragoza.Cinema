namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Showtime.Domain
{
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using Xunit;

    public sealed class TicketTests
    {
        [Fact]
        public void MarkAsPaid_WhenHavingUnPaidTicket_ShouldMarkTicketAsPaid()
        {
            // Arrange
            var ticket = CreateTicketUtils.Create();

            // Act
            ticket.MarkAsPurchased();

            // Assert
            ticket.IsPurchased.Should().Be(true);
        }

        [Fact]
        public void MarkAsPaid_WhenHavingAPaidTicket_ShouldMarkTicketAsPaid()
        {
            // Arrange
            var ticket = CreateTicketUtils.Create();

            ticket.MarkAsPurchased();

            // Act
            ticket.MarkAsPurchased();

            // Assert
            ticket.IsPurchased.Should().Be(true);
        }
    }
}