namespace JordiAragon.Cinema.Reservation.UnitTests.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using Xunit;

    public class TicketTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateTicket()
        {
            var auditorium = CreateAuditoriumUtils.Create();
            var seatIds = auditorium.Seats.Select(seat => seat.Id);
            var createdTimeOnUtc = DateTime.UtcNow;

            var idValues = new object[] { null, Constants.Ticket.Id };
            var seatIdsValues = new object[] { null, new List<SeatId>(), seatIds };
            var createdTimeOnUtcValues = new object[] { default(DateTime), createdTimeOnUtc };

            foreach (var idValue in idValues)
            {
                foreach (var seatIdsValue in seatIdsValues)
                {
                    foreach (var createdTimeOnUtcValue in createdTimeOnUtcValues)
                    {
                        if (idValue != null && idValue.Equals(Constants.Ticket.Id) &&
                            seatIdsValue == seatIds &&
                            createdTimeOnUtcValue.Equals(createdTimeOnUtc))
                        {
                            continue;
                        }

                        yield return new object[] { idValue, seatIdsValue, createdTimeOnUtcValue };
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateTicket))]
        public void CreateTicket_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            TicketId id,
            IEnumerable<SeatId> seatIds,
            DateTime createdTimeOnUtc)
        {
            // Act
            Func<Ticket> createTicket = () => Ticket.Create(id, seatIds, createdTimeOnUtc);

            // Assert
            createTicket.Should().Throw<ArgumentException>();
        }

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