namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;
    using Xunit;
    using Showtime = JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Showtime;

    public sealed class ShowtimeTests
    {
        public static IEnumerable<object[]> InvalidArgumentsScheduleShowtime()
        {
            var showtimeId = Constants.Showtime.Id;
            var movieId = Constants.Movie.Id;
            var sessionDateOnUtc = Constants.Showtime.SessionDateOnUtc;
            var auditoriumId = Constants.Showtime.AuditoriumId;

            var showtimeIdValues = new object[] { default!, showtimeId };
            var movieIdValues = new object[] { default!, movieId };
            var sessionDateOnUtcValues = new object[] { default!, sessionDateOnUtc };
            var auditoriumIdValues = new object[] { default!, auditoriumId };

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
                                    sessionDateOnUtcValue != default && sessionDateOnUtcValue.Equals(sessionDateOnUtc) &&
                                    auditoriumIdValue != null && auditoriumIdValue.Equals(auditoriumId))
                            {
                                continue;
                            }

                            yield return new object[] { showtimeIdValue!, movieIdValue!, sessionDateOnUtcValue!, auditoriumIdValue! };
                        }
                    }
                }
            }
        }

        public static IEnumerable<object[]> InvalidArgumentsReserveSeats()
        {
            var ticketId = Constants.Ticket.Id;
            var userId = Constants.Ticket.UserId;
            var seatIds = new List<SeatId> { Constants.Seat.Id };
            var reservationDateOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            var ticketIdValues = new object[] { default!, ticketId };
            var userIdValues = new object[] { default!, userId };
            var seatIdsValues = new object[] { default!, new List<SeatId>(), seatIds };
            var createdTimeOnUtcValues = new object[] { default!, reservationDateOnUtc };

            foreach (var ticketIdValue in ticketIdValues)
            {
                foreach (var userIdValue in userIdValues)
                {
                    foreach (var seatIdsValue in seatIdsValues)
                    {
                        foreach (var createdTimeOnUtcValue in createdTimeOnUtcValues)
                        {
                            if (ticketIdValue != null && ticketIdValue.Equals(ticketId) &&
                                userIdValue != null && userIdValue.Equals(userId) &&
                                seatIdsValue != null && seatIdsValue.Equals(seatIds) &&
                                createdTimeOnUtcValue != null && createdTimeOnUtcValue.Equals(reservationDateOnUtc))
                            {
                                continue;
                            }

                            yield return new object[] { ticketIdValue!, userIdValue!, seatIdsValue!, createdTimeOnUtcValue! };
                        }
                    }
                }
            }
        }

        [Fact]
        public void ScheduleShowtime_WhenHavingValidArguments_ShouldScheduleShowtimeAndAddShowtimeCreatedEvent()
        {
            // Arrange
            var id = Constants.Showtime.Id;
            var movieId = Constants.Movie.Id;
            var sessionDateOnUtc = Constants.Showtime.SessionDateOnUtc;
            var auditoriumId = Constants.Showtime.AuditoriumId;

            // Act
            var showtime = Showtime.Schedule(id, movieId, sessionDateOnUtc, auditoriumId);

            // Assert
            showtime.Should().NotBeNull();
            showtime.Id.Should().Be(id);

            showtime.Events.Should()
                              .ContainSingle(x => x is ShowtimeScheduledEvent)
                              .Which.Should().BeOfType<ShowtimeScheduledEvent>()
                              .Which.Should().Match<ShowtimeScheduledEvent>(e =>
                                                                            e.AggregateId == id &&
                                                                            e.MovieId == movieId &&
                                                                            e.SessionDateOnUtc == sessionDateOnUtc &&
                                                                            e.AuditoriumId == auditoriumId);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsScheduleShowtime))]
        public void ScheduleShowtime_WhenHavingInvalidArguments_ShouldThrowException(
            ShowtimeId id,
            MovieId movieId,
            SessionDate sessionDateOnUtc,
            AuditoriumId auditoriumId)
        {
            // Act
            Func<Showtime> showtime = () => Showtime.Schedule(id, movieId, sessionDateOnUtc, auditoriumId);

            // Assert
            showtime.Should().Throw<Exception>();
        }

        [Fact]
        public void Cancel_WhenHavingValidArguments_ShouldCreateShowtimeCanceledEvent()
        {
            // Arrange
            var showtime = ScheduleShowtimeUtils.Schedule();

            // Act
            showtime.Cancel();

            // Assert
            showtime.Events.Should()
                              .ContainSingle(x => x is ShowtimeCanceledEvent)
                              .Which.Should().BeOfType<ShowtimeCanceledEvent>()
                              .Which.Should().Match<ShowtimeCanceledEvent>(e =>
                                                                            e.AggregateId == showtime.Id &&
                                                                            e.AuditoriumId == showtime.AuditoriumId &&
                                                                            e.MovieId == showtime.MovieId);
        }

        [Fact]
        public void End_WhenHavingValidArguments_ShouldCreateShowtimeEndedEvent()
        {
            // Arrange
            var showtime = ScheduleShowtimeUtils.Schedule();

            // Act
            showtime.End();

            // Assert
            showtime.Events.Should()
                              .ContainSingle(x => x is ShowtimeEndedEvent)
                              .Which.Should().BeOfType<ShowtimeEndedEvent>()
                              .Which.Should().Match<ShowtimeEndedEvent>(e =>
                                                                            e.AggregateId == showtime.Id);
        }

        [Fact]
        public void ReserveSeats_WhenHavingValidArguments_ShouldCreateTicketAddTheTickedAndAddReservedSeatsEvent()
        {
            // Arrange
            var showtime = ScheduleShowtimeUtils.Schedule();
            var ticketId = Constants.Ticket.Id;
            var userId = Constants.Ticket.UserId;

            var seatIds = new List<SeatId>
            {
                Constants.Seat.Id,
            };

            var reservationDateOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            // Act
            var ticketCreated = showtime.ReserveSeats(ticketId, userId, seatIds, reservationDateOnUtc);

            // Assert
            ticketCreated.Should().NotBeNull();
            ticketCreated.Seats.Should().BeEquivalentTo(seatIds);
            ticketCreated.ReservationDateOnUtc.Should().Be(reservationDateOnUtc);
            ticketCreated.IsPurchased.Should().Be(false);

            showtime.Tickets.Should().HaveCount(1).And.Contain(ticketCreated);

            showtime.Events.Should()
                              .ContainSingle(x => x is ReservedSeatsEvent)
                              .Which.Should().BeOfType<ReservedSeatsEvent>()
                              .Which.Should().Match<ReservedSeatsEvent>(e =>
                                                                            e.AggregateId == showtime.Id &&
                                                                            e.TicketId == ticketCreated.Id &&
                                                                            e.SeatIds.Count() == seatIds.Count &&
                                                                            e.SeatIds.All(id => seatIds.Contains(new SeatId(id))) &&
                                                                            e.CreatedTimeOnUtc == reservationDateOnUtc);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsReserveSeats))]
        public void ReserveSeats_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            TicketId tickedId,
            UserId userId,
            IEnumerable<SeatId> seatIds,
            ReservationDate reservationDateOnUtc)
        {
            // Arrange
            var showtime = ScheduleShowtimeUtils.Schedule();

            // Act
            Func<Ticket> ticketCreated = () => showtime.ReserveSeats(tickedId, userId, seatIds, reservationDateOnUtc);

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

            var userId = Constants.Ticket.UserId;

            var showtime = ScheduleShowtimeUtils.Schedule();

            var seatIds = new List<SeatId>
            {
                Constants.Seat.Id,
            };

            var reservationDateOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            var ticketCreated = showtime.ReserveSeats(ticketId, userId, seatIds, reservationDateOnUtc);

            // Act
            showtime.PurchaseTicket(ticketId);

            // Assert
            ticketCreated.IsPurchased.Should().BeTrue();

            showtime.Events.Should()
                              .ContainSingle(x => x is PurchasedTicketEvent)
                              .Which.Should().BeOfType<PurchasedTicketEvent>()
                              .Which.Should().Match<PurchasedTicketEvent>(e =>
                                                                            e.AggregateId == showtime.Id &&
                                                                            e.TicketId == ticketCreated.Id);
        }

        [Fact]
        public void PurchaseSeats_WhenHavingNullTicketId_ShouldThrowArgumentException()
        {
            // Arrange
            TicketId ticketId = null!;

            var showtime = ScheduleShowtimeUtils.Schedule();

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

            var showtime = ScheduleShowtimeUtils.Schedule();

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

            var userId = Constants.Ticket.UserId;

            var showtime = ScheduleShowtimeUtils.Schedule();

            var seatIds = new List<SeatId>
            {
                Constants.Seat.Id,
            };

            var reservationDateOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            showtime.ReserveSeats(ticketId, userId, seatIds, reservationDateOnUtc);

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

            var userId = Constants.Ticket.UserId;

            var showtime = ScheduleShowtimeUtils.Schedule();

            var seatIds = new List<SeatId>
            {
                Constants.Seat.Id,
            };

            var reservationDateOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            showtime.ReserveSeats(ticketId, userId, seatIds, reservationDateOnUtc);

            // Act
            showtime.ExpireReservedSeats(ticketId);

            // Assert
            showtime.Tickets.Should().BeEmpty();

            showtime.Events.Should()
                              .ContainSingle(x => x is ExpiredReservedSeatsEvent)
                              .Which.Should().BeOfType<ExpiredReservedSeatsEvent>()
                              .Which.Should().Match<ExpiredReservedSeatsEvent>(e =>
                                                                            e.AggregateId == showtime.Id &&
                                                                            e.TicketId == ticketId);
        }

        [Fact]
        public void ExpireReservedSeats_WhenHavingNullTicketId_ShouldThrowArgumentException()
        {
            // Arrange
            TicketId ticketId = null!;

            var showtime = ScheduleShowtimeUtils.Schedule();

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

            var showtime = ScheduleShowtimeUtils.Schedule();

            // Act
            Action showtimePurchaseSeats = () => showtime.ExpireReservedSeats(ticketId);

            // Assert
            showtimePurchaseSeats.Should().Throw<NotFoundException>();

            showtime.Events.Should()
                           .NotContain(x => x is ExpiredReservedSeatsEvent);
        }
    }
}