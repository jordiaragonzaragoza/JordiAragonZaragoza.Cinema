namespace JordiAragon.Cinemas.Ticketing.Domain.UnitTests.ShowtimeAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate;
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate;
    using JordiAragon.Cinemas.Ticketing.Domain.UnitTests.Features.Auditorium.TestUtils;
    using JordiAragon.Cinemas.Ticketing.Domain.UnitTests.Features.Showtime.TestUtils;
    using JordiAragon.Cinemas.Ticketing.Domain.UnitTests.TestUtils.Constants;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using Xunit;

    using Auditorium = JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate.Auditorium;
    using Seat = JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate.Seat;
    using Showtime = JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate.Showtime;

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
                                                 .Select(seat => seat.Id).ToList();
            var ticketId = Constants.Ticket.Id;
            var createdTimeOnUtc = DateTime.UtcNow;

            var auditoriumValues = new object[] { null, auditorium };
            var showtimeValues = new object[] { null, showtime };
            var desiredSeatIdsValues = new object[] { null, new List<SeatId>(), desiredSeatIds };
            var ticketIdValues = new object[] { null, ticketId };
            var createdTimeOnUtcValues = new object[] { default(DateTime), createdTimeOnUtc };

            foreach (var auditoriumValue in auditoriumValues)
            {
                foreach (var showtimeValue in showtimeValues)
                {
                    foreach (var desiredSeatIdsValue in desiredSeatIdsValues)
                    {
                        foreach (var ticketIdValue in ticketIdValues)
                        {
                            foreach (var createdTimeOnUtcValue in createdTimeOnUtcValues)
                            {
                                if (auditoriumValue != null && auditoriumValue.Equals(auditorium) &&
                                    showtimeValue != null && showtimeValue.Equals(showtime) &&
                                    desiredSeatIdsValue != null && desiredSeatIdsValue.Equals(desiredSeatIds) &&
                                    ticketIdValue != null && ticketIdValue.Equals(ticketId) &&
                                    createdTimeOnUtcValue.Equals(createdTimeOnUtc))
                                {
                                    continue;
                                }

                                yield return new object[] { auditoriumValue, showtimeValue, desiredSeatIdsValue, ticketIdValue, createdTimeOnUtcValue };
                            }
                        }
                    }
                }
            }
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
            FluentActions.Invoking(() => ShowtimeManager.ReserveSeats(auditorium, showtime, desiredSeatIds, ticketId, createdTimeOnUtc))
            .Should().Throw<ArgumentException>();
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