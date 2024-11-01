namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Showtime.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using Xunit;

    public sealed class TicketIdTests
    {
        [Fact]
        public void CreateTicketId_WhenHavingAnEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<TicketId> ticketId = () => TicketId.Create(id);

            // Assert
            ticketId.Should().Throw<ArgumentException>();
        }
    }
}