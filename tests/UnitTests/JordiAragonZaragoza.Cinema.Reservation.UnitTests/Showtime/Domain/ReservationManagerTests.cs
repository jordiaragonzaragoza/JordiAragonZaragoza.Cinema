namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;
    using NSubstitute;
    using Xunit;

    using Auditorium = JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Auditorium;
    using Showtime = JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Showtime;

    public sealed class ReservationManagerTests
    {
        private readonly ReservationManager showtimeManager;

        private readonly IReadRepository<Auditorium, AuditoriumId> mockAuditoriumRepository;
        private readonly IReadRepository<Movie, MovieId> mockMovieRepository;

        public ReservationManagerTests()
        {
            this.mockAuditoriumRepository = Substitute.For<IReadRepository<Auditorium, AuditoriumId>>();
            this.mockMovieRepository = Substitute.For<IReadRepository<Movie, MovieId>>();

            this.showtimeManager = new ReservationManager(
                this.mockMovieRepository,
                this.mockAuditoriumRepository);
        }

        public static IEnumerable<object[]> InvalidArgumentsShowtimeManagerConstructor()
        {
            var auditoriumRepository = Substitute.For<IReadRepository<Auditorium, AuditoriumId>>();
            var movieRepository = Substitute.For<IReadRepository<Movie, MovieId>>();

            var auditoriumRepositoryValues = new object[] { default!, auditoriumRepository };
            var movieRepositoryValues = new object[] { default!, movieRepository };

            foreach (var auditoriumRepositoryValue in auditoriumRepositoryValues)
            {
                foreach (var movieRepositoryValue in movieRepositoryValues)
                {
                    if (auditoriumRepositoryValue != null && auditoriumRepositoryValue.Equals(auditoriumRepository) &&
                                        movieRepositoryValue != null && movieRepositoryValue.Equals(movieRepository))
                    {
                        continue;
                    }

                    yield return new object[] { auditoriumRepositoryValue!, movieRepositoryValue! };
                }
            }
        }

        public static IEnumerable<object[]> InvalidArgumentsReserveSeatsAsync()
        {
            var showtime = ScheduleShowtimeUtils.Schedule();
            var desiredSeatIds = new List<SeatId> { Constants.Seat.Id };
            var newTicketId = Constants.Ticket.Id;
            var userId = Constants.Ticket.UserId;
            var reservationDate = ReservationDate.Create(DateTimeOffset.UtcNow);

            var showtimeValues = new object[] { default!, showtime };
            var desiredSeatIdsValues = new object[] { default!, new List<SeatId>(), desiredSeatIds };
            var newTicketIdValues = new object[] { default!, newTicketId };
            var userIdValues = new object[] { default!, userId };
            var currentDateTimeOnUtcValues = new object[] { default!, reservationDate };

            foreach (var showtimeValue in showtimeValues)
            {
                foreach (var desiredSeatIdsValue in desiredSeatIdsValues)
                {
                    foreach (var newTicketIdValue in newTicketIdValues)
                    {
                        foreach (var userIdValue in userIdValues)
                        {
                            foreach (var currentDateTimeOnUtcValue in currentDateTimeOnUtcValues)
                            {
                                if (showtimeValue != null && showtimeValue.Equals(showtime) &&
                                    desiredSeatIdsValue != null && desiredSeatIdsValue.Equals(desiredSeatIds) &&
                                    newTicketIdValue != null && newTicketIdValue.Equals(newTicketId) &&
                                    userIdValue != null && userIdValue.Equals(userId) &&
                                    currentDateTimeOnUtcValue != null && currentDateTimeOnUtcValue.Equals(reservationDate))
                                {
                                    continue;
                                }

                                yield return new object[] { showtimeValue!, desiredSeatIdsValue!, newTicketIdValue!, userIdValue!, currentDateTimeOnUtcValue! };
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<object[]> InvalidArgumentsHasShowtimeEndedAsync()
        {
            var showtime = ScheduleShowtimeUtils.Schedule();
            var currentDateTimeOnUtc = DateTimeOffset.UtcNow;

            var showtimeValues = new object[] { default!, showtime };
            var currentDateTimeOnUtcValues = new object[] { default(DateTimeOffset), currentDateTimeOnUtc };

            foreach (var showtimeValue in showtimeValues)
            {
                foreach (var currentDateTimeOnUtcValue in currentDateTimeOnUtcValues)
                {
                    if (showtimeValue != null && showtimeValue.Equals(showtime) &&
                        currentDateTimeOnUtcValue != default && currentDateTimeOnUtcValue.Equals(currentDateTimeOnUtc))
                    {
                        continue;
                    }

                    yield return new object[] { showtimeValue!, currentDateTimeOnUtcValue! };
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsShowtimeManagerConstructor))]
        public void ConstructorShowtimeManager_WhenHavingInvalidArguments_ShouldThrowArgumentNullException(
            IReadRepository<Auditorium, AuditoriumId> auditoriumRepository,
            IReadRepository<Movie, MovieId> movieRepository)
        {
            FluentActions.Invoking(() => new ReservationManager(movieRepository, auditoriumRepository))
            .Should().Throw<ArgumentException>();
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsReserveSeatsAsync))]
        public void ReserveSeats_WhenHavingInvalidArguments_ShouldThrowArgumentNullException(
            Showtime showtime,
            IEnumerable<SeatId> desiredSeatIds,
            TicketId newTicketId,
            UserId userId,
            ReservationDate currentDateTimeOnUtc)
        {
            FluentActions.Invoking(async () => await this.showtimeManager.ReserveSeatsAsync(
                showtime,
                desiredSeatIds,
                newTicketId,
                userId,
                currentDateTimeOnUtc,
                CancellationToken.None))
            .Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task ReserveSeats_WhenHavingValidArguments_ShouldCreateATicket()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();
            var movie = CreateMovieUtils.Create();

            var showtime = ScheduleShowtimeUtils.Schedule();
            var desiredSeatIds = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3)
                                                 .Select(seat => seat.Id);

            var ticketId = Constants.Ticket.Id;
            var userId = Constants.Ticket.UserId;
            var createdTimeOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            this.mockMovieRepository.GetByIdAsync(Arg.Any<MovieId>(), Arg.Any<CancellationToken>())
                .Returns(movie);

            this.mockAuditoriumRepository.GetByIdAsync(Arg.Any<AuditoriumId>(), Arg.Any<CancellationToken>())
                .Returns(auditorium);

            // Act
            var ticketCreated = await this.showtimeManager.ReserveSeatsAsync(
                showtime,
                desiredSeatIds,
                ticketId,
                userId,
                createdTimeOnUtc,
                CancellationToken.None);

            // Assert
            ticketCreated.Should().NotBeNull();
            ticketCreated.Seats.Should().BeEquivalentTo(desiredSeatIds);
            ticketCreated.ReservationDateOnUtc.Should().Be(createdTimeOnUtc);
        }

        [Fact]
        public void ReserveSeats_WhenHavingUnExistingMovie_ShouldThrowNotFoundException()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();
            Movie movie = null!;

            var showtime = Showtime.Schedule(
                Constants.Showtime.Id,
                Constants.Showtime.MovieId,
                SessionDate.Create(DateTimeOffset.UtcNow.AddDays(-1)),
                Constants.Showtime.AuditoriumId);

            var desiredSeatIds = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3)
                                                 .Select(seat => seat.Id).ToList();

            var ticketId = Constants.Ticket.Id;
            var userId = Constants.Ticket.UserId;

            var currentDateTimeOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            this.mockMovieRepository.GetByIdAsync(Arg.Any<MovieId>(), Arg.Any<CancellationToken>())
                .Returns(movie);

            // Act
            Func<Task> sut = async () => await this.showtimeManager.ReserveSeatsAsync(
                showtime,
                desiredSeatIds,
                ticketId,
                userId,
                currentDateTimeOnUtc,
                CancellationToken.None);

            // Assert
            sut.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public void ReserveSeats_WhenHavingUnExistingAuditorium_ShouldThrowNotFoundException()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();
            var movie = CreateMovieUtils.Create();

            var showtime = Showtime.Schedule(
                Constants.Showtime.Id,
                Constants.Showtime.MovieId,
                SessionDate.Create(DateTimeOffset.UtcNow.AddDays(-1)),
                Constants.Showtime.AuditoriumId);

            var desiredSeatIds = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3)
                                                 .Select(seat => seat.Id).ToList();

            var ticketId = Constants.Ticket.Id;
            var userId = Constants.Ticket.UserId;

            var currentDateTimeOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            this.mockMovieRepository.GetByIdAsync(Arg.Any<MovieId>(), Arg.Any<CancellationToken>())
                .Returns(movie);

            this.mockAuditoriumRepository.GetByIdAsync(Arg.Any<AuditoriumId>(), Arg.Any<CancellationToken>())
                .Returns((Auditorium)null!);

            // Act
            Func<Task> sut = async () => await this.showtimeManager.ReserveSeatsAsync(
                showtime,
                desiredSeatIds,
                ticketId,
                userId,
                currentDateTimeOnUtc,
                CancellationToken.None);

            // Assert
            sut.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public void ReserveSeats_WhenHavingAnEndedShowtime_ShouldThrowBusinessRuleValidationException()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();
            var movie = CreateMovieUtils.Create();

            var showtime = Showtime.Schedule(
                Constants.Showtime.Id,
                Constants.Showtime.MovieId,
                SessionDate.Create(DateTimeOffset.UtcNow.AddDays(-1)),
                Constants.Showtime.AuditoriumId);

            var desiredSeatIds = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3)
                                                 .Select(seat => seat.Id).ToList();
            var ticketId = Constants.Ticket.Id;
            var userId = Constants.Ticket.UserId;
            var currentDateTimeOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            this.mockMovieRepository.GetByIdAsync(Arg.Any<MovieId>(), Arg.Any<CancellationToken>())
                .Returns(movie);

            this.mockAuditoriumRepository.GetByIdAsync(Arg.Any<AuditoriumId>(), Arg.Any<CancellationToken>())
                .Returns(auditorium);

            // Act
            Func<Task> sut = async () => await this.showtimeManager.ReserveSeatsAsync(
                showtime,
                desiredSeatIds,
                ticketId,
                userId,
                currentDateTimeOnUtc,
                CancellationToken.None);

            // Assert
            sut.Should().ThrowAsync<BusinessRuleValidationException>().WithMessage("No reservations are allowed after showtime ended.");
        }

        [Fact]
        public void ReserveSeats_WhenHavingNotContiguousSeats_ShouldThrowBusinessRuleValidationException()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();
            var movie = CreateMovieUtils.Create();

            var showtime = ScheduleShowtimeUtils.Schedule();
            var desiredSeatIds = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3)
                                                 .Select(seat => seat.Id).ToList();
            desiredSeatIds.RemoveAt(1);

            var ticketId = Constants.Ticket.Id;
            var userId = Constants.Ticket.UserId;
            var createdTimeOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            this.mockMovieRepository.GetByIdAsync(Arg.Any<MovieId>(), Arg.Any<CancellationToken>())
                .Returns(movie);

            this.mockAuditoriumRepository.GetByIdAsync(Arg.Any<AuditoriumId>(), Arg.Any<CancellationToken>())
                .Returns(auditorium);

            // Act
            Func<Task> sut = async () => await this.showtimeManager.ReserveSeatsAsync(
                showtime,
                desiredSeatIds,
                ticketId,
                userId,
                createdTimeOnUtc,
                CancellationToken.None);

            // Assert
            sut.Should().ThrowAsync<BusinessRuleValidationException>().WithMessage("Only contiguous seats can be reserved.");
        }

        [Fact]
        public async Task ReserveSeats_WhenTryToReserveAReservedSeat_ShouldThrowBusinessRuleValidationException()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();
            var movie = CreateMovieUtils.Create();

            var showtime = ScheduleShowtimeUtils.Schedule();
            var desiredSeatIds = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3)
                                                 .Select(seat => seat.Id);

            var ticketId = Constants.Ticket.Id;
            var userId = Constants.Ticket.UserId;
            var createdTimeOnUtc = ReservationDate.Create(DateTimeOffset.UtcNow);

            this.mockMovieRepository.GetByIdAsync(Arg.Any<MovieId>(), Arg.Any<CancellationToken>())
                .Returns(movie);

            this.mockAuditoriumRepository.GetByIdAsync(Arg.Any<AuditoriumId>(), Arg.Any<CancellationToken>())
                .Returns(auditorium);

            await this.showtimeManager.ReserveSeatsAsync(
                showtime,
                desiredSeatIds,
                ticketId,
                userId,
                createdTimeOnUtc,
                CancellationToken.None);

            // Act
            Func<Task> sut = async () => await this.showtimeManager.ReserveSeatsAsync(
                showtime,
                desiredSeatIds,
                ticketId,
                userId,
                createdTimeOnUtc,
                CancellationToken.None);

            // Assert
            await sut.Should().ThrowAsync<BusinessRuleValidationException>().Where(e => e.Message.Contains("Only available seats can be reserved"));
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsHasShowtimeEndedAsync))]
        public void HasShowtimeEndedAsync_WhenHavingInvalidArguments_ShouldThrowArgumentNullException(
            Showtime showtime,
            DateTimeOffset currentDateTimeOnUtc)
        {
            FluentActions.Invoking(async () => await this.showtimeManager.HasShowtimeEndedAsync(
                showtime,
                currentDateTimeOnUtc,
                CancellationToken.None))
            .Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public void HasShowtimeEndedAsync_WhenHavingUnExistingShowtime_ShouldThrowNotFoundException()
        {
            // Arrange
            var showtime = Showtime.Schedule(
                Constants.Showtime.Id,
                Constants.Showtime.MovieId,
                SessionDate.Create(DateTimeOffset.UtcNow.AddDays(-1)),
                Constants.Showtime.AuditoriumId);

            var currentDateTimeOnUtc = DateTimeOffset.UtcNow;

            Movie movie = default!;

            this.mockMovieRepository.GetByIdAsync(Arg.Any<MovieId>(), Arg.Any<CancellationToken>())
                .Returns(movie);

            // Act
            Func<Task> sut = async () => await this.showtimeManager.HasShowtimeEndedAsync(
                showtime,
                currentDateTimeOnUtc,
                CancellationToken.None);

            // Assert
            sut.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task HasShowtimeEndedAsync_WhenHavingAnEndedShowtime_ShouldReturnTrue()
        {
            // Arrange
            var showtime = Showtime.Schedule(
                Constants.Showtime.Id,
                Constants.Showtime.MovieId,
                SessionDate.Create(DateTimeOffset.UtcNow.AddDays(-1)),
                Constants.Showtime.AuditoriumId);

            var currentDateTimeOnUtc = DateTimeOffset.UtcNow;

            var movie = CreateMovieUtils.Create();

            this.mockMovieRepository.GetByIdAsync(Arg.Any<MovieId>(), Arg.Any<CancellationToken>())
                .Returns(movie);

            // Act
            var result = await this.showtimeManager.HasShowtimeEndedAsync(showtime, currentDateTimeOnUtc, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task HasShowtimeEndedAsync_WhenHavingANotEndedShowtime_ShouldReturnFalse()
        {
            // Arrange
            var showtime = Showtime.Schedule(
                Constants.Showtime.Id,
                Constants.Showtime.MovieId,
                SessionDate.Create(DateTimeOffset.UtcNow),
                Constants.Showtime.AuditoriumId);

            var currentDateTimeOnUtc = DateTimeOffset.UtcNow.AddDays(-1);

            var movie = CreateMovieUtils.Create();

            this.mockMovieRepository.GetByIdAsync(Arg.Any<MovieId>(), Arg.Any<CancellationToken>())
                .Returns(movie);

            // Act
            var result = await this.showtimeManager.HasShowtimeEndedAsync(showtime, currentDateTimeOnUtc, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}