namespace JordiAragon.Cinema.Reservation.UnitTests.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using Xunit;
    using Showtime = JordiAragon.Cinema.Reservation.Showtime.Domain.Showtime;

    public class ShowtimeTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateShowtime()
        {
            var showtimeId = Constants.Showtime.Id;
            var movieId = Constants.Movie.Id;
            var sessionDateOnUtc = Constants.Showtime.SessionDateOnUtc;
            var auditoriumId = Constants.Showtime.AuditoriumId;

            var showtimeIdValues = new object[] { null, showtimeId };
            var movieIdValues = new object[] { null, movieId };
            var sessionDateOnUtcValues = new object[] { default(DateTimeOffset), sessionDateOnUtc };
            var auditoriumIdValues = new object[] { null, auditoriumId };

            foreach (var showtimeIdValue in showtimeIdValues)
            {
                foreach (var movieIdValue in movieIdValues)
                {
                    foreach (var sessionDateOnUtcValue in sessionDateOnUtcValues)
                    {
                        foreach (var auditoriumIdValue in auditoriumIdValues)
                        {
                            if (showtimeIdValue != null && showtimeIdValue.Equals(showtimeId) &&
                                    movieIdValue != null && movieIdValue.Equals(movieId) &&
                                    sessionDateOnUtcValue.Equals(sessionDateOnUtc) &&
                                    auditoriumIdValue != null && auditoriumIdValue.Equals(auditoriumId))
                            {
                                continue;
                            }

                            yield return new object[] { showtimeIdValue, movieIdValue, sessionDateOnUtcValue, auditoriumIdValue };
                        }
                    }
                }
            }
        }

        public static IEnumerable<object[]> InvalidArgumentsReserveSeats()
        {
            var ticketId = Constants.Ticket.Id;
            var seatIds = new List<SeatId> { Constants.Seat.Id };
            var createdTimeOnUtc = DateTime.UtcNow;

            var ticketIdValues = new object[] { null, ticketId };
            var seatIdsValues = new object[] { null, new List<SeatId>(), seatIds };
            var createdTimeOnUtcValues = new object[] { default(DateTimeOffset), createdTimeOnUtc };

            foreach (var ticketIdValue in ticketIdValues)
            {
                foreach (var seatIdsValue in seatIdsValues)
                {
                    foreach (var createdTimeOnUtcValue in createdTimeOnUtcValues)
                    {
                        if (ticketIdValue != null && ticketIdValue.Equals(ticketId) &&
                                    seatIdsValue != null && seatIdsValue.Equals(seatIds) &&
                                    createdTimeOnUtcValue.Equals(createdTimeOnUtc))
                        {
                            continue;
                        }

                        yield return new object[] { ticketIdValue, seatIdsValue, createdTimeOnUtcValue };
                    }
                }
            }
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
                              .ContainSingle(x => x is ShowtimeCreatedEvent)
                              .Which.Should().BeOfType<ShowtimeCreatedEvent>()
                              .Which.Should().Match<ShowtimeCreatedEvent>(e =>
                                                                            e.ShowtimeId == id &&
                                                                            e.MovieId == movieId &&
                                                                            e.SessionDateOnUtc == sessionDateOnUtc &&
                                                                            e.AuditoriumId == auditoriumId);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateShowtime))]
        public void CreateShowtime_WhenHavingInvalidArguments_ShouldThrowException(
            ShowtimeId id,
            MovieId movieId,
            DateTimeOffset sessionDateOnUtc,
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
            ticketCreated.IsPurchased.Should().Be(false);

            showtime.Tickets.Should().HaveCount(1).And.Contain(ticketCreated);

            showtime.Events.Should()
                              .ContainSingle(x => x is ReservedSeatsEvent)
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
            DateTimeOffset createdTimeOnUtc)
        {
            // Arrange
            var showtime = CreateShowtimeUtils.Create();

            // Act
            Func<Ticket> ticketCreated = () => showtime.ReserveSeats(tickedId, seatIds, createdTimeOnUtc);

            // Assert
            ticketCreated.Should().Throw<ArgumentException>();

            showtime.Tickets.Should().BeEmpty();

            showtime.Events.Should()
                              .NotContain(x => x is ReservedSeatsEvent);
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
            showtime.PurchaseTicket(ticketId);

            // Assert
            ticketCreated.IsPurchased.Should().BeTrue();

            showtime.Events.Should()
                              .ContainSingle(x => x is PurchasedTicketEvent)
                              .Which.Should().BeOfType<PurchasedTicketEvent>()
                              .Which.Should().Match<PurchasedTicketEvent>(e =>
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
            Action showtimePurchaseSeats = () => showtime.PurchaseTicket(ticketId);

            // Assert
            showtimePurchaseSeats.Should().Throw<ArgumentException>();

            showtime.Events.Should()
                           .NotContain(x => x is PurchasedTicketEvent);
        }

        [Fact]
        public void PurchaseSeats_WhenHavingInvalidTicketId_ShouldThrowNotFoundException()
        {
            // Arrange
            var ticketId = Constants.Ticket.Id;

            var showtime = CreateShowtimeUtils.Create();

            // Act
            Action showtimePurchaseSeats = () => showtime.PurchaseTicket(ticketId);

            // Assert
            showtimePurchaseSeats.Should().Throw<NotFoundException>();

            showtime.Events.Should()
                           .NotContain(x => x is PurchasedTicketEvent);
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

            showtime.PurchaseTicket(ticketId);

            // Act
            Action showtimePurchaseSeats = () => showtime.PurchaseTicket(ticketId);

            // Assert
            showtimePurchaseSeats.Should().Throw<BusinessRuleValidationException>().WithMessage("Only possible to purchase once per ticket.");
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
                              .ContainSingle(x => x is ExpiredReservedSeatsEvent)
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
                           .NotContain(x => x is ExpiredReservedSeatsEvent);
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
                           .NotContain(x => x is ExpiredReservedSeatsEvent);
        }
    }
}