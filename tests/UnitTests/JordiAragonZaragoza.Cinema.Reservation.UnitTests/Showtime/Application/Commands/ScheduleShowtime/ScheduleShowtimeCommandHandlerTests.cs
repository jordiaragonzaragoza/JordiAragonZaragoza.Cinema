﻿namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Showtime.Application.Commands.ScheduleShowtime
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.ScheduleShowtime;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using NSubstitute;
    using Xunit;

    public sealed class ScheduleShowtimeCommandHandlerTests
    {
        private readonly ScheduleShowtimeCommandHandler handler;

        private readonly IRepository<Auditorium, AuditoriumId> mockAuditoriumRepository;
        private readonly IRepository<Movie, MovieId> mockMovieRepository;
        private readonly IRepository<Showtime, ShowtimeId> mockShowtimeRepository;

        public ScheduleShowtimeCommandHandlerTests()
        {
            this.mockAuditoriumRepository = Substitute.For<IRepository<Auditorium, AuditoriumId>>();
            this.mockMovieRepository = Substitute.For<IRepository<Movie, MovieId>>();
            this.mockShowtimeRepository = Substitute.For<IRepository<Showtime, ShowtimeId>>();

            this.handler = new ScheduleShowtimeCommandHandler(
                this.mockAuditoriumRepository,
                this.mockMovieRepository,
                this.mockShowtimeRepository);
        }

        public static IEnumerable<object[]> InvalidArgumentsCreateHandleScheduleShowtimeCommand()
        {
            var auditoriumRepository = Substitute.For<IRepository<Auditorium, AuditoriumId>>();
            var movieRepository = Substitute.For<IRepository<Movie, MovieId>>();
            var showtimeRepository = Substitute.For<IRepository<Showtime, ShowtimeId>>();

            var auditoriumRepositoryValues = new object[] { default!, auditoriumRepository };
            var movieRepositoryValues = new object[] { default!, movieRepository };
            var showtimeRepositoryValues = new object[] { default!, showtimeRepository };

            foreach (var auditoriumRepositoryValue in auditoriumRepositoryValues)
            {
                foreach (var movieRepositoryValue in movieRepositoryValues)
                {
                    foreach (var showtimeRepositoryValue in showtimeRepositoryValues)
                    {
                        if (auditoriumRepositoryValue != null && auditoriumRepositoryValue.Equals(auditoriumRepository) &&
                                    movieRepositoryValue != null && movieRepositoryValue.Equals(movieRepository) &&
                                    showtimeRepositoryValue != null && showtimeRepositoryValue.Equals(showtimeRepository))
                            {
                                continue;
                            }

                        yield return new object[] { auditoriumRepositoryValue!, movieRepositoryValue!, showtimeRepositoryValue! };
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateHandleScheduleShowtimeCommand))]
        public void CreateHandleScheduleShowtimeCommand_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            IRepository<Auditorium, AuditoriumId> auditoriumRepository,
            IRepository<Movie, MovieId> movieRepository,
            IRepository<Showtime, ShowtimeId> showtimeRepository)
        {
            FluentActions.Invoking(() => new ScheduleShowtimeCommandHandler(auditoriumRepository, movieRepository, showtimeRepository))
            .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleScheduleShowtimeCommand_WhenShowtimeIsValid_ShouldCreateAndReturnShowtimeGuid()
        {
            // Arrange
            var createShowtimeCommand = ScheduleShowtimeCommandUtils.CreateCommand();

            var existingMovie = CreateMovieUtils.Create();
            var existingAuditorium = CreateAuditoriumUtils.Create();

            this.mockAuditoriumRepository.GetByIdAsync(Arg.Any<AuditoriumId>(), Arg.Any<CancellationToken>())
                .Returns(existingAuditorium);

            this.mockMovieRepository.GetByIdAsync(Arg.Any<MovieId>(), Arg.Any<CancellationToken>())
                .Returns(existingMovie);

            // Act
            var result = await this.handler.Handle(createShowtimeCommand, default);

            // Assert
            // 1. Validate correct showtime created based on command.
            result.IsSuccess.Should().BeTrue();
            result.Should().NotBeNull();

            // 2. Some showtime was added to the repository.
            await this.mockShowtimeRepository.Received(1).AddAsync(Arg.Any<Showtime>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleScheduleShowtimeCommand_WhenAuditoriumNotExist_ShouldReturnAError()
        {
            // Arrange
            var createShowtimeCommand = ScheduleShowtimeCommandUtils.CreateCommand();

            Auditorium auditorium = null!;

            this.mockAuditoriumRepository.GetByIdAsync(Arg.Any<AuditoriumId>(), Arg.Any<CancellationToken>())
                .Returns(auditorium);

            // Act
            var result = await this.handler.Handle(createShowtimeCommand, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Should().NotBeNull();

            await this.mockShowtimeRepository.Received(0).AddAsync(Arg.Any<Showtime>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleScheduleShowtimeCommand_WhenMovieNotExist_ShouldReturnAError()
        {
            // Arrange
            var createShowtimeCommand = ScheduleShowtimeCommandUtils.CreateCommand();

            var existingAuditorium = CreateAuditoriumUtils.Create();

            this.mockAuditoriumRepository.GetByIdAsync(Arg.Any<AuditoriumId>(), Arg.Any<CancellationToken>())
                .Returns(existingAuditorium);

            Movie movie = default!;

            this.mockMovieRepository.GetByIdAsync(Arg.Any<MovieId>(), Arg.Any<CancellationToken>())
                .Returns(movie);

            // Act
            var result = await this.handler.Handle(createShowtimeCommand, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Should().NotBeNull();

            await this.mockShowtimeRepository.Received(0).AddAsync(Arg.Any<Showtime>(), Arg.Any<CancellationToken>());
        }
    }
}