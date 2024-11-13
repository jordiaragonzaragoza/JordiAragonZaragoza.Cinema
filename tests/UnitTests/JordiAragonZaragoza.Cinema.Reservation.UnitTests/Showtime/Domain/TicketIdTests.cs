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
            Func<TicketId> ticketId = () => new TicketId(id);

            // Assert
            ticketId.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ImplicitConversion_WhenHavingATicketId_ShouldReturnGuid()
        {
            // Arrange
            var value = Guid.NewGuid();
            var ticketId = new TicketId(value);

            // Act
            Guid result = ticketId;

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfTicketId()
        {
            // Arrange
            var value = Guid.NewGuid();
            var ticketId = new TicketId(value);

            // Act
            var result = ticketId.ToString();

            // Assert
            result.Should().Be(value.ToString());
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            var value1 = Guid.NewGuid();
            var value2 = Guid.NewGuid();

            var ticketId1 = new TicketId(value1);
            var ticketId2 = new TicketId(value1);
            var ticketId3 = new TicketId(value2);

            // Act & Assert
            ticketId1.Should().Be(ticketId2);
            ticketId1.Should().NotBe(ticketId3);
        }
    }
}