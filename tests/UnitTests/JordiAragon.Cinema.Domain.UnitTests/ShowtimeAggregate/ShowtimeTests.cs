namespace JordiAragon.Cinema.Domain.UnitTests.ShowtimeAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate.Events;
    using JordiAragon.Cinema.Domain.UnitTests.Features.Showtime.TestUtils;
    using JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using Xunit;

    using Showtime = JordiAragon.Cinema.Domain.ShowtimeAggregate.Showtime;

    public class ShowtimeTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateShowtime()
        {
            yield return new object[] { null, null, default(DateTime), null };
            yield return new object[] { null, null, default(DateTime), Constants.Showtime.AuditoriumId };
            yield return new object[] { null, null, Constants.Showtime.SessionDateOnUtc, null };
            yield return new object[] { null, null, Constants.Showtime.SessionDateOnUtc, Constants.Showtime.AuditoriumId };
            yield return new object[] { null, Constants.Movie.Id, default(DateTime), null };
            yield return new object[] { null, Constants.Movie.Id, default(DateTime), Constants.Showtime.AuditoriumId };
            yield return new object[] { null, Constants.Movie.Id, Constants.Showtime.SessionDateOnUtc, null };
            yield return new object[] { null, Constants.Movie.Id, Constants.Showtime.SessionDateOnUtc, Constants.Showtime.AuditoriumId };
            yield return new object[] { Constants.Showtime.Id, null, Constants.Showtime.SessionDateOnUtc, Constants.Showtime.AuditoriumId };
            yield return new object[] { Constants.Showtime.Id, Constants.Movie.Id, Constants.Showtime.SessionDateOnUtc, null };
            yield return new object[] { Constants.Showtime.Id, null, Constants.Showtime.SessionDateOnUtc, null };
            yield return new object[] { Constants.Showtime.Id, Constants.Movie.Id, default(DateTime), Constants.Showtime.AuditoriumId };
            yield return new object[] { Constants.Showtime.Id, null, default(DateTime), Constants.Showtime.AuditoriumId };
            yield return new object[] { Constants.Showtime.Id, Constants.Movie.Id, default(DateTime), null };
            yield return new object[] { Constants.Showtime.Id, null, default(DateTime), null };
        }

        public static IEnumerable<object[]> InvalidArgumentsReserveSeats()
        {
            yield return new object[] { null, null, default(DateTime) };
            yield return new object[] { Constants.Ticket.Id, null, default(DateTime) };
            yield return new object[] { null, new List<SeatId> { Constants.Seat.Id }, default(DateTime) };
            yield return new object[] { null, new List<SeatId>(), default(DateTime) };
            yield return new object[] { Constants.Ticket.Id, new List<SeatId> { Constants.Seat.Id }, default(DateTime) };
            yield return new object[] { Constants.Ticket.Id, new List<SeatId>(), default(DateTime) };
            yield return new object[] { null, new List<SeatId> { Constants.Seat.Id }, DateTime.UtcNow };
            yield return new object[] { null, new List<SeatId>(), DateTime.UtcNow };
            yield return new object[] { Constants.Ticket.Id, null, DateTime.UtcNow };
            yield return new object[] { null, null, DateTime.UtcNow };
        }

        [Fact]
        public void CreateShowtime_WhenHavingValidArguments_ShouldCreateShowtimeAndAddShowtimeCreatedEvent()
        {
            // Arrange
            var id = Constants.Showtime.Id;
            var movieId = Constants.Movie.Id;
            var sessionDateOnUtc = Constants.Showtime.SessionDateOnUtc;
            var auditoriumId = Constants.Showtime.AuditoriumId;

            // Act
            var showtime = Showtime.Create(id, movieId, sessionDateOnUtc, auditoriumId);

            // Assert
            showtime.Should().NotBeNull();
            showtime.Id.Should().Be(id);

            showtime.Events.Should()
                              .ContainSingle(x => x.GetType() == typeof(ShowtimeCreatedEvent))
                              .Which.Should().BeOfType<ShowtimeCreatedEvent>()
                              .Which.Should().Match<ShowtimeCreatedEvent>(e =>
                                                                            e.ShowtimeId == id &&
                                                                            e.MovieId == movieId &&
                                                                            e.SessionDateOnUtc == sessionDateOnUtc &&
                                                                            e.AuditoriumId == auditoriumId);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateShowtime))]
        public void CreateShowtime_WhenHavingInvalidArguments_ShouldShouldThrowException(
            ShowtimeId id,
            MovieId movieId,
            DateTime sessionDateOnUtc,
            AuditoriumId auditoriumId)
        {
            // Act
            Func<Showtime> showtime = () => Showtime.Create(id, movieId, sessionDateOnUtc, auditoriumId);

            // Assert
            showtime.Should().Throw<Exception>();
        }

        [Fact]
        public void ReserveSeats_WhenHavingValidArguments_ShouldCreateTicketAddTheTickedAndAddReservedSeatsEvent()
        {
            // Arrange
            var showtime = CreateShowtimeUtils.Create();
            var ticketId = Constants.Ticket.Id;

            var seatIds = new List<SeatId>
            {
                Constants.Seat.Id,
            };

            var createdTimeOnUtc = DateTime.UtcNow;

            // Act
            var ticketCreated = showtime.ReserveSeats(ticketId, seatIds, createdTimeOnUtc);

            // Assert
            ticketCreated.Should().NotBeNull();
            ticketCreated.Seats.Should().BeEquivalentTo(seatIds);
            ticketCreated.CreatedTimeOnUtc.Should().Be(createdTimeOnUtc);
            ticketCreated.IsPaid.Should().Be(false);

            showtime.Tickets.Should().HaveCount(1).And.Contain(ticketCreated);

            showtime.Events.Should()
                              .ContainSingle(x => x.GetType() == typeof(ReservedSeatsEvent))
                              .Which.Should().BeOfType<ReservedSeatsEvent>()
                              .Which.Should().Match<ReservedSeatsEvent>(e =>
                                                                            e.ShowtimeId == showtime.Id &&
                                                                            e.TicketId == ticketCreated.Id &&
                                                                            e.SeatIds.Count() == seatIds.Count &&
                                                                            e.SeatIds.All(id => seatIds.Contains(SeatId.Create(id))) &&
                                                                            e.CreatedTimeOnUtc == createdTimeOnUtc);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsReserveSeats))]
        public void ReserveSeats_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            TicketId tickedId,
            IEnumerable<SeatId> seatIds,
            DateTime createdTimeOnUtc)
        {
            // Arrange
            var showtime = CreateShowtimeUtils.Create();

            // Act
            Func<Ticket> ticketCreated = () => showtime.ReserveSeats(tickedId, seatIds, createdTimeOnUtc);

            // Assert
            ticketCreated.Should().Throw<ArgumentException>();

            showtime.Tickets.Should().BeEmpty();

            showtime.Events.Should()
                              .NotContain(x => x.GetType() == typeof(ReservedSeatsEvent));
        }

        [Fact]
        public void PurchaseSeats_WhenHavingValidTicketId_ShouldMarkTicketAsPaidAndAddPurchasedSeatsEvent()
        {
            // Arrange
            var ticketId = Constants.Ticket.Id;

            var showtime = CreateShowtimeUtils.Create();

            var seatIds = new List<SeatId>
            {
                Constants.Seat.Id,
            };

            var createdTimeOnUtc = DateTime.UtcNow;

            var ticketCreated = showtime.ReserveSeats(ticketId, seatIds, createdTimeOnUtc);

            // Act
            showtime.PurchaseSeats(ticketId);

            // Assert
            ticketCreated.IsPaid.Should().BeTrue();

            showtime.Events.Should()
                              .ContainSingle(x => x.GetType() == typeof(PurchasedSeatsEvent))
                              .Which.Should().BeOfType<PurchasedSeatsEvent>()
                              .Which.Should().Match<PurchasedSeatsEvent>(e =>
                                                                            e.ShowtimeId == showtime.Id &&
                                                                            e.TicketId == ticketCreated.Id);
        }

        [Fact]
        public void PurchaseSeats_WhenHavingNullTicketId_ShouldThrowArgumentException()
        {
            // Arrange
            TicketId ticketId = null;

            var showtime = CreateShowtimeUtils.Create();

            // Act
            Action showtimePurchaseSeats = () => showtime.PurchaseSeats(ticketId);

            // Assert
            showtimePurchaseSeats.Should().Throw<ArgumentException>();

            showtime.Events.Should()
                           .NotContain(x => x.GetType() == typeof(PurchasedSeatsEvent));
        }

        [Fact]
        public void PurchaseSeats_WhenHavingInvalidTicketId_ShouldThrowNotFoundException()
        {
            // Arrange
            var ticketId = Constants.Ticket.Id;

            var showtime = CreateShowtimeUtils.Create();

            // Act
            Action showtimePurchaseSeats = () => showtime.PurchaseSeats(ticketId);

            // Assert
            showtimePurchaseSeats.Should().Throw<NotFoundException>();

            showtime.Events.Should()
                           .NotContain(x => x.GetType() == typeof(PurchasedSeatsEvent));
        }

        [Fact]
        public void PurchaseSeats_WhenHavingAPurchasedTicketId_ShouldThrowBusinessRuleValidationException()
        {
            // Arrange
            var ticketId = Constants.Ticket.Id;

            var showtime = CreateShowtimeUtils.Create();

            var seatIds = new List<SeatId>
            {
                Constants.Seat.Id,
            };

            var createdTimeOnUtc = DateTime.UtcNow;

            showtime.ReserveSeats(ticketId, seatIds, createdTimeOnUtc);

            showtime.PurchaseSeats(ticketId);

            // Act
            Action showtimePurchaseSeats = () => showtime.PurchaseSeats(ticketId);

            // Assert
            showtimePurchaseSeats.Should().Throw<BusinessRuleValidationException>().WithMessage("Only possible to pay once per reservation.");
        }

        [Fact]
        public void ExpireReservedSeats_WhenHavingValidTickedId_ShouldRemoveTheTicketAndAddExpiredReservedSeatsEvent()
        {
            // Arrange
            var ticketId = Constants.Ticket.Id;

            var showtime = CreateShowtimeUtils.Create();

            var seatIds = new List<SeatId>
            {
                Constants.Seat.Id,
            };

            var createdTimeOnUtc = DateTime.UtcNow;

            showtime.ReserveSeats(ticketId, seatIds, createdTimeOnUtc);

            // Act
            showtime.ExpireReservedSeats(ticketId);

            // Assert
            showtime.Tickets.Should().BeEmpty();

            showtime.Events.Should()
                              .ContainSingle(x => x.GetType() == typeof(ExpiredReservedSeatsEvent))
                              .Which.Should().BeOfType<ExpiredReservedSeatsEvent>()
                              .Which.Should().Match<ExpiredReservedSeatsEvent>(e =>
                                                                            e.ShowtimeId == showtime.Id &&
                                                                            e.TicketId == ticketId);
        }

        [Fact]
        public void ExpireReservedSeats_WhenHavingNullTicketId_ShouldThrowArgumentException()
        {
            // Arrange
            TicketId ticketId = null;

            var showtime = CreateShowtimeUtils.Create();

            // Act
            Action showtimePurchaseSeats = () => showtime.ExpireReservedSeats(ticketId);

            // Assert
            showtimePurchaseSeats.Should().Throw<ArgumentException>();

            showtime.Events.Should()
                           .NotContain(x => x.GetType() == typeof(ExpiredReservedSeatsEvent));
        }

        [Fact]
        public void ExpireReservedSeats_WhenHavingInvalidTicketId_ShouldThrowNotFoundException()
        {
            // Arrange
            var ticketId = Constants.Ticket.Id;

            var showtime = CreateShowtimeUtils.Create();

            // Act
            Action showtimePurchaseSeats = () => showtime.ExpireReservedSeats(ticketId);

            // Assert
            showtimePurchaseSeats.Should().Throw<NotFoundException>();

            showtime.Events.Should()
                           .NotContain(x => x.GetType() == typeof(ExpiredReservedSeatsEvent));
        }
    }
}