namespace JordiAragon.Cinemas.Ticketing.Domain.UnitTests.ShowtimeAggregate
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate;
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