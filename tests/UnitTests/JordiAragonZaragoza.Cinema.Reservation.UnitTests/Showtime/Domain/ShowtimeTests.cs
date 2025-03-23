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
            var reservationId = Constants.Reservation.Id;
            var userId = Constants.Reservation.UserId;
            var seatIds = new List<SeatId> { Constants.Seat.Id };
            var reservationDateOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            var reservationIdValues = new object[] { default!, reservationId };
            var userIdValues = new object[] { default!, userId };
            var seatIdsValues = new object[] { default!, new List<SeatId>(), seatIds };
            var createdTimeOnUtcValues = new object[] { default!, reservationDateOnUtc };

            foreach (var reservationIdValue in reservationIdValues)
            {
                foreach (var userIdValue in userIdValues)
                {
                    foreach (var seatIdsValue in seatIdsValues)
                    {
                        foreach (var createdTimeOnUtcValue in createdTimeOnUtcValues)
                        {
                            if (reservationIdValue != null && reservationIdValue.Equals(reservationId) &&
                                userIdValue != null && userIdValue.Equals(userId) &&
                                seatIdsValue != null && seatIdsValue.Equals(seatIds) &&
                                createdTimeOnUtcValue != null && createdTimeOnUtcValue.Equals(reservationDateOnUtc))
                            {
                                continue;
                            }

                            yield return new object[] { reservationIdValue!, userIdValue!, seatIdsValue!, createdTimeOnUtcValue! };
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
        public void ReserveSeats_WhenHavingValidArguments_ShouldCreateReservationAddTheTickedAndAddReservedSeatsEvent()
        {
            // Arrange
            var showtime = ScheduleShowtimeUtils.Schedule();
            var reservationId = Constants.Reservation.Id;
            var userId = Constants.Reservation.UserId;

            var seatIds = new List<SeatId>
            {
                Constants.Seat.Id,
            };

            var reservationDateOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            // Act
            var reservationCreated = showtime.ReserveSeats(reservationId, userId, seatIds, reservationDateOnUtc);

            // Assert
            reservationCreated.Should().NotBeNull();
            reservationCreated.Seats.Should().BeEquivalentTo(seatIds);
            reservationCreated.ReservationDateOnUtc.Should().Be(reservationDateOnUtc);
            reservationCreated.IsPurchased.Should().Be(false);

            showtime.Reservations.Should().HaveCount(1).And.Contain(reservationCreated);

            showtime.Events.Should()
                              .ContainSingle(x => x is ReservedSeatsEvent)
                              .Which.Should().BeOfType<ReservedSeatsEvent>()
                              .Which.Should().Match<ReservedSeatsEvent>(e =>
                                                                            e.AggregateId == showtime.Id &&
                                                                            e.ReservationId == reservationCreated.Id &&
                                                                            e.SeatIds.Count() == seatIds.Count &&
                                                                            e.SeatIds.All(id => seatIds.Contains(new SeatId(id))) &&
                                                                            e.CreatedTimeOnUtc == reservationDateOnUtc);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsReserveSeats))]
        public void ReserveSeats_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            ReservationId tickedId,
            UserId userId,
            IEnumerable<SeatId> seatIds,
            ReservationDate reservationDateOnUtc)
        {
            // Arrange
            var showtime = ScheduleShowtimeUtils.Schedule();

            // Act
            Func<Reservation> reservationCreated = () => showtime.ReserveSeats(tickedId, userId, seatIds, reservationDateOnUtc);

            // Assert
            reservationCreated.Should().Throw<ArgumentException>();

            showtime.Reservations.Should().BeEmpty();

            showtime.Events.Should()
                              .NotContain(x => x is ReservedSeatsEvent);
        }

        [Fact]
        public void PurchaseSeats_WhenHavingValidReservationId_ShouldMarkReservationAsPaidAndAddPurchasedSeatsEvent()
        {
            // Arrange
            var reservationId = Constants.Reservation.Id;

            var userId = Constants.Reservation.UserId;

            var showtime = ScheduleShowtimeUtils.Schedule();

            var seatIds = new List<SeatId>
            {
                Constants.Seat.Id,
            };

            var reservationDateOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            var reservationCreated = showtime.ReserveSeats(reservationId, userId, seatIds, reservationDateOnUtc);

            // Act
            showtime.PurchaseReservation(reservationId);

            // Assert
            reservationCreated.IsPurchased.Should().BeTrue();

            showtime.Events.Should()
                              .ContainSingle(x => x is PurchasedReservationEvent)
                              .Which.Should().BeOfType<PurchasedReservationEvent>()
                              .Which.Should().Match<PurchasedReservationEvent>(e =>
                                                                            e.AggregateId == showtime.Id &&
                                                                            e.ReservationId == reservationCreated.Id);
        }

        [Fact]
        public void PurchaseSeats_WhenHavingNullReservationId_ShouldThrowArgumentException()
        {
            // Arrange
            ReservationId reservationId = null!;

            var showtime = ScheduleShowtimeUtils.Schedule();

            // Act
            Action showtimePurchaseSeats = () => showtime.PurchaseReservation(reservationId);

            // Assert
            showtimePurchaseSeats.Should().Throw<ArgumentException>();

            showtime.Events.Should()
                           .NotContain(x => x is PurchasedReservationEvent);
        }

        [Fact]
        public void PurchaseSeats_WhenHavingInvalidReservationId_ShouldThrowNotFoundException()
        {
            // Arrange
            var reservationId = Constants.Reservation.Id;

            var showtime = ScheduleShowtimeUtils.Schedule();

            // Act
            Action showtimePurchaseSeats = () => showtime.PurchaseReservation(reservationId);

            // Assert
            showtimePurchaseSeats.Should().Throw<NotFoundException>();

            showtime.Events.Should()
                           .NotContain(x => x is PurchasedReservationEvent);
        }

        [Fact]
        public void PurchaseSeats_WhenHavingAPurchasedReservationId_ShouldThrowBusinessRuleValidationException()
        {
            // Arrange
            var reservationId = Constants.Reservation.Id;

            var userId = Constants.Reservation.UserId;

            var showtime = ScheduleShowtimeUtils.Schedule();

            var seatIds = new List<SeatId>
            {
                Constants.Seat.Id,
            };

            var reservationDateOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            showtime.ReserveSeats(reservationId, userId, seatIds, reservationDateOnUtc);

            showtime.PurchaseReservation(reservationId);

            // Act
            Action showtimePurchaseSeats = () => showtime.PurchaseReservation(reservationId);

            // Assert
            showtimePurchaseSeats.Should().Throw<BusinessRuleValidationException>().WithMessage("Only possible to purchase once per reservation.");
        }

        [Fact]
        public void ExpireReservedSeats_WhenHavingValidTickedId_ShouldRemoveTheReservationAndAddExpiredReservedSeatsEvent()
        {
            // Arrange
            var reservationId = Constants.Reservation.Id;

            var userId = Constants.Reservation.UserId;

            var showtime = ScheduleShowtimeUtils.Schedule();

            var seatIds = new List<SeatId>
            {
                Constants.Seat.Id,
            };

            var reservationDateOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            showtime.ReserveSeats(reservationId, userId, seatIds, reservationDateOnUtc);

            // Act
            showtime.ExpireReservedSeats(reservationId);

            // Assert
            showtime.Reservations.Should().BeEmpty();

            showtime.Events.Should()
                              .ContainSingle(x => x is ExpiredReservedSeatsEvent)
                              .Which.Should().BeOfType<ExpiredReservedSeatsEvent>()
                              .Which.Should().Match<ExpiredReservedSeatsEvent>(e =>
                                                                            e.AggregateId == showtime.Id &&
                                                                            e.ReservationId == reservationId);
        }

        [Fact]
        public void ExpireReservedSeats_WhenHavingNullReservationId_ShouldThrowArgumentException()
        {
            // Arrange
            ReservationId reservationId = null!;

            var showtime = ScheduleShowtimeUtils.Schedule();

            // Act
            Action showtimePurchaseSeats = () => showtime.ExpireReservedSeats(reservationId);

            // Assert
            showtimePurchaseSeats.Should().Throw<ArgumentException>();

            showtime.Events.Should()
                           .NotContain(x => x is ExpiredReservedSeatsEvent);
        }

        [Fact]
        public void ExpireReservedSeats_WhenHavingInvalidReservationId_ShouldThrowNotFoundException()
        {
            // Arrange
            var reservationId = Constants.Reservation.Id;

            var showtime = ScheduleShowtimeUtils.Schedule();

            // Act
            Action showtimePurchaseSeats = () => showtime.ExpireReservedSeats(reservationId);

            // Assert
            showtimePurchaseSeats.Should().Throw<NotFoundException>();

            showtime.Events.Should()
                           .NotContain(x => x is ExpiredReservedSeatsEvent);
        }
    }
}