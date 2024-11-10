namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using Xunit;

    public sealed class TicketTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateTicket()
        {
            var auditorium = CreateAuditoriumUtils.Create();
            var seatIds = auditorium.Seats.Select(seat => seat.Id);
            var reservationDate = ReservationDate.Create(DateTimeOffset.UtcNow);

            var idValues = new object[] { default!, Constants.Ticket.Id };
            var userIdValues = new object[] { default!, Constants.Ticket.UserId };
            var seatIdsValues = new object[] { default!, new List<SeatId>(), seatIds };
            var createdTimeOnUtcValues = new object[] { default!, reservationDate };

            foreach (var idValue in idValues)
            {
                foreach (var userIdValue in userIdValues)
                {
                    foreach (var seatIdsValue in seatIdsValues)
                    {
                        foreach (var createdTimeOnUtcValue in createdTimeOnUtcValues)
                        {
                            if (idValue != null && idValue.Equals(Constants.Ticket.Id) &&
                                userIdValue != null && userIdValue.Equals(Constants.Ticket.UserId) &&
                                seatIdsValue != null && seatIdsValue == seatIds &&
                                createdTimeOnUtcValue != null && createdTimeOnUtcValue.Equals(reservationDate))
                            {
                                continue;
                            }

                            yield return new object[] { idValue!, userIdValue!, seatIdsValue!, createdTimeOnUtcValue! };
                        }
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateTicket))]
        public void CreateTicket_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            TicketId id,
            UserId userId,
            IEnumerable<SeatId> seatIds,
            ReservationDate createdTimeOnUtc)
        {
            // Act
            Func<Ticket> createTicket = () => new Ticket(id, userId, seatIds, createdTimeOnUtc);

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