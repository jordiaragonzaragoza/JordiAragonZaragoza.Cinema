namespace JordiAragon.Cinemas.Ticketing.UnitTests.Showtime.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain;
    using Xunit;

    public class TicketIdTests
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