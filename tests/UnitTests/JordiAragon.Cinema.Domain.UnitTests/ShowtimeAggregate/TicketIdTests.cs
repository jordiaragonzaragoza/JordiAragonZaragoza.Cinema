namespace JordiAragon.Cinema.Domain.UnitTests.ShowtimeAggregate
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
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