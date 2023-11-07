namespace JordiAragon.Cinema.Reservation.UnitTests.Showtime.Domain.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Specifications;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using Xunit;

    using Showtime = JordiAragon.Cinema.Reservation.Showtime.Domain.Showtime;

    public class ShowtimeByTicketIdSpecTests
    {
        [Fact]
        public void FindShowtimeByTicketIdSpec_WhenHavingAnValidTicketId_ShouldReturnTheShowtime()
        {
            // Arrange
            var ticketId = Constants.Ticket.Id;
            var auditorium = CreateAuditoriumUtils.Create();
            var showtime1 = CreateShowtimeUtils.Create();
            showtime1.ReserveSeats(ticketId, auditorium.Seats.Select(seat => seat.Id), DateTime.UtcNow);

            var showtime2 = CreateShowtimeUtils.Create();

            var showtimes = new List<Showtime>() { showtime1, showtime2 };

            var specification = new ShowtimeByTicketIdSpec(ticketId);

            // Act
            var evaluatedList = specification.Evaluate(showtimes);

            // Assert
            evaluatedList.Should()
                         .ContainSingle(c => c == showtime1 && showtime1.Tickets.Any(ticket => ticket.Id == ticketId))
                         .And.NotContain(c => c == showtime2);
        }

        [Fact]
        public void FindShowtimeByTicketIdSpec_WhenHavingAnInvalidTicketId_ShouldNotContainTheTicket()
        {
            // Arrange
            var ticketId = TicketId.Create(Guid.NewGuid());
            var auditorium = CreateAuditoriumUtils.Create();
            var showtime = CreateShowtimeUtils.Create();
            showtime.ReserveSeats(Constants.Ticket.Id, auditorium.Seats.Select(seat => seat.Id), DateTime.UtcNow);
            var showtimes = new List<Showtime>() { showtime };

            var specification = new ShowtimeByTicketIdSpec(ticketId);

            // Act
            var evaluatedList = specification.Evaluate(showtimes);

            // Assert
            evaluatedList.Should().BeEmpty();
            evaluatedList.Should()
                         .NotContain(c => c == showtime);
        }

        [Fact]
        public void FindShowtimeByTicketIdSpec_WhenHavingAnInvalidArgument_ShouldThrowArgumentException()
        {
            // Arrange
            TicketId ticketId = null;

            // Act
            Func<ShowtimeByTicketIdSpec> showtime = () => new ShowtimeByTicketIdSpec(ticketId);

            // Assert
            showtime.Should().Throw<ArgumentException>();
        }
    }
}