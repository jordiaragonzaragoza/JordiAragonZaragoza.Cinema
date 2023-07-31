namespace JordiAragon.Cinema.Domain.UnitTests.ShowtimeAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.Cinema.Domain.UnitTests.Features.Auditorium.TestUtils;
    using JordiAragon.Cinema.Domain.UnitTests.Features.Showtime.TestUtils;
    using JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using Xunit;

    using Auditorium = JordiAragon.Cinema.Domain.AuditoriumAggregate.Auditorium;
    using Seat = JordiAragon.Cinema.Domain.AuditoriumAggregate.Seat;
    using Showtime = JordiAragon.Cinema.Domain.ShowtimeAggregate.Showtime;

    public class ShowtimeManagerTests
    {
        public static IEnumerable<object[]> InvalidArgumentsAvailableSeats()
        {
            yield return new object[] { null, null };
            yield return new object[] { CreateAuditoriumUtils.Create(), null };
            yield return new object[] { null, CreateShowtimeUtils.Create() };
        }

        public static IEnumerable<object[]> InvalidArgumentsReserveSeats()
        {
            var auditorium = CreateAuditoriumUtils.Create();
            var showtime = CreateShowtimeUtils.Create();
            var desiredSeatIds = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3)
                                                 .Select(seat => seat.Id);
            var ticketId = Constants.Ticket.Id;
            var createdTimeOnUtc = DateTime.UtcNow;

            yield return new object[] { null, null, null, null, default(DateTime) };
            yield return new object[] { null, null, null, ticketId, default(DateTime) };
            yield return new object[] { null, null, desiredSeatIds, null, default(DateTime) };
            yield return new object[] { null, null, desiredSeatIds, ticketId, default(DateTime) };
            yield return new object[] { null, showtime, null, null, default(DateTime) };
            yield return new object[] { null, showtime, null, ticketId, default(DateTime) };
            yield return new object[] { null, showtime, desiredSeatIds, null, default(DateTime) };
            yield return new object[] { null, showtime, desiredSeatIds, ticketId, default(DateTime) };
            yield return new object[] { auditorium, null, null, null, default(DateTime) };
            yield return new object[] { auditorium, null, null, ticketId, default(DateTime) };
            yield return new object[] { auditorium, null, desiredSeatIds, null, default(DateTime) };
            yield return new object[] { auditorium, null, desiredSeatIds, ticketId, default(DateTime) };
            yield return new object[] { auditorium, showtime, null, null, default(DateTime) };
            yield return new object[] { auditorium, showtime, null, ticketId, default(DateTime) };
            yield return new object[] { auditorium, showtime, desiredSeatIds, null, default(DateTime) };
            yield return new object[] { auditorium, showtime, desiredSeatIds, ticketId, default(DateTime) };
            yield return new object[] { null, null, null, null, createdTimeOnUtc };
            yield return new object[] { null, null, null, ticketId, createdTimeOnUtc };
            yield return new object[] { null, null, desiredSeatIds, null, createdTimeOnUtc };
            yield return new object[] { null, null, desiredSeatIds, ticketId, createdTimeOnUtc };
            yield return new object[] { null, showtime, null, null, createdTimeOnUtc };
            yield return new object[] { null, showtime, null, ticketId, createdTimeOnUtc };
            yield return new object[] { null, showtime, desiredSeatIds, null, createdTimeOnUtc };
            yield return new object[] { null, showtime, desiredSeatIds, ticketId, createdTimeOnUtc };
            yield return new object[] { auditorium, null, null, null, createdTimeOnUtc };
            yield return new object[] { auditorium, null, null, ticketId, createdTimeOnUtc };
            yield return new object[] { auditorium, null, desiredSeatIds, null, createdTimeOnUtc };
            yield return new object[] { auditorium, null, desiredSeatIds, ticketId, createdTimeOnUtc };
            yield return new object[] { auditorium, showtime, null, null, createdTimeOnUtc };
            yield return new object[] { auditorium, showtime, null, ticketId, createdTimeOnUtc };
            yield return new object[] { auditorium, showtime, desiredSeatIds, null, createdTimeOnUtc };
            yield return new object[] { null, null, new List<SeatId>(), null, default(DateTime) };
            yield return new object[] { null, null, new List<SeatId>(), ticketId, default(DateTime) };
            yield return new object[] { null, showtime, new List<SeatId>(), null, default(DateTime) };
            yield return new object[] { null, showtime, new List<SeatId>(), ticketId, default(DateTime) };
            yield return new object[] { auditorium, null, new List<SeatId>(), null, default(DateTime) };
            yield return new object[] { auditorium, null, new List<SeatId>(), ticketId, default(DateTime) };
            yield return new object[] { auditorium, showtime, new List<SeatId>(), null, default(DateTime) };
            yield return new object[] { auditorium, showtime, new List<SeatId>(), ticketId, default(DateTime) };
            yield return new object[] { null, null, new List<SeatId>(), null, createdTimeOnUtc };
            yield return new object[] { null, null, new List<SeatId>(), ticketId, createdTimeOnUtc };
            yield return new object[] { null, showtime, new List<SeatId>(), null, createdTimeOnUtc };
            yield return new object[] { null, showtime, new List<SeatId>(), ticketId, createdTimeOnUtc };
            yield return new object[] { auditorium, null, new List<SeatId>(), null, createdTimeOnUtc };
            yield return new object[] { auditorium, null, new List<SeatId>(), ticketId, createdTimeOnUtc };
            yield return new object[] { auditorium, showtime, new List<SeatId>(), null, createdTimeOnUtc };
        }

        [Fact]
        public void AvailableSeats_WhenHavingValidArguments_ShouldReturnAvailableSeats()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();

            var showtime = CreateShowtimeUtils.Create();

            // Act
            var availableSeats = ShowtimeManager.AvailableSeats(auditorium, showtime);

            // Assert
            availableSeats.Count().Should().Be(Constants.Auditorium.Rows * Constants.Auditorium.SeatsPerRow);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsAvailableSeats))]
        public void AvailableSeats_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            Auditorium auditorium,
            Showtime showtime)
        {
            // Act
            Func<IEnumerable<Seat>> seats = () => ShowtimeManager.AvailableSeats(auditorium, showtime);

            // Assert
            seats.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ReserveSeats_WhenHavingValidArguments_ShouldCreateATicket()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();

            var showtime = CreateShowtimeUtils.Create();

            var desiredSeatIds = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3)
                                                 .Select(seat => seat.Id);

            var ticketId = Constants.Ticket.Id;

            var createdTimeOnUtc = DateTime.UtcNow;

            // Act
            var ticketCreated = ShowtimeManager.ReserveSeats(auditorium, showtime, desiredSeatIds, ticketId, createdTimeOnUtc);

            // Assert
            ticketCreated.Should().NotBeNull();
            ticketCreated.Seats.Should().BeEquivalentTo(desiredSeatIds);
            ticketCreated.CreatedTimeOnUtc.Should().Be(createdTimeOnUtc);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsReserveSeats))]
        public void ReserveSeats_WhenHavingInvalidArguments_ShouldThrowArgumentNullException(
            Auditorium auditorium,
            Showtime showtime,
            IEnumerable<SeatId> desiredSeatIds,
            TicketId ticketId,
            DateTime createdTimeOnUtc)
        {
            // Act
            Func<Ticket> ticketCreated = () => ShowtimeManager.ReserveSeats(auditorium, showtime, desiredSeatIds, ticketId, createdTimeOnUtc);

            // Assert
            ticketCreated.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ReserveSeats_WhenHavingNotContiguousSeats_ShouldThrowBusinessRuleValidationException()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();

            var showtime = CreateShowtimeUtils.Create();

            var desiredSeatIds = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3)
                                                 .Select(seat => seat.Id).ToList();
            desiredSeatIds.RemoveAt(1);

            var ticketId = Constants.Ticket.Id;

            var createdTimeOnUtc = DateTime.UtcNow;

            // Act
            Func<Ticket> ticketCreated = () => ShowtimeManager.ReserveSeats(auditorium, showtime, desiredSeatIds, ticketId, createdTimeOnUtc);

            // Assert
            ticketCreated.Should().Throw<BusinessRuleValidationException>().WithMessage("Only contiguous seats can be reserved.");
        }

        [Fact]
        public void ReserveSeats_WhenTryToReserveAReservedSeat_ShouldThrowBusinessRuleValidationException()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();

            var showtime = CreateShowtimeUtils.Create();

            var desiredSeatIds = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(2)
                                                 .Select(seat => seat.Id).ToList();

            var ticketId = Constants.Ticket.Id;

            var createdTimeOnUtc = DateTime.UtcNow;

            ShowtimeManager.ReserveSeats(auditorium, showtime, desiredSeatIds, ticketId, createdTimeOnUtc);

            // Act
            Func<Ticket> ticketCreated = () => ShowtimeManager.ReserveSeats(auditorium, showtime, desiredSeatIds, ticketId, createdTimeOnUtc);

            // Assert
            ticketCreated.Should().Throw<BusinessRuleValidationException>().Where(e => e.Message.Contains("Only available seats can be reserved"));
        }
    }
}