namespace JordiAragon.Cinema.Reservation.UnitTests.Showtime.Application.Commands.ScheduleShowtime
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.CommandHandlers.ScheduleShowtime;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Application;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using NSubstitute;
    using Volo.Abp.Guids;
    using Xunit;

    public sealed class ScheduleShowtimeCommandHandlerTests
    {
        private readonly ScheduleShowtimeCommandHandler handler;

        private readonly IRepository<Auditorium, AuditoriumId> mockAuditoriumRepository;
        private readonly IRepository<Movie, MovieId> mockMovieRepository;
        private readonly IRepository<Showtime, ShowtimeId> mockShowtimeRepository;
        private readonly IGuidGenerator mockGuidGenerator;

        public ScheduleShowtimeCommandHandlerTests()
        {
            this.mockAuditoriumRepository = Substitute.For<IRepository<Auditorium, AuditoriumId>>();
            this.mockMovieRepository = Substitute.For<IRepository<Movie, MovieId>>();
            this.mockShowtimeRepository = Substitute.For<IRepository<Showtime, ShowtimeId>>();
            this.mockGuidGenerator = Substitute.For<IGuidGenerator>();

            this.handler = new ScheduleShowtimeCommandHandler(
                this.mockAuditoriumRepository,
                this.mockMovieRepository,
                this.mockShowtimeRepository,
                this.mockGuidGenerator);
        }

        public static IEnumerable<object[]> InvalidArgumentsCreateHandleScheduleShowtimeCommand()
        {
            var auditoriumRepository = Substitute.For<IRepository<Auditorium, AuditoriumId>>();
            var movieRepository = Substitute.For<IRepository<Movie, MovieId>>();
            var showtimeRepository = Substitute.For<IRepository<Showtime, ShowtimeId>>();
            var guidGenerator = Substitute.For<IGuidGenerator>();

            var auditoriumRepositoryValues = new object[] { null, auditoriumRepository };
            var movieRepositoryValues = new object[] { null, movieRepository };
            var showtimeRepositoryValues = new object[] { null, showtimeRepository };
            var guidGeneratorValues = new object[] { null, guidGenerator };

            foreach (var auditoriumRepositoryValue in auditoriumRepositoryValues)
            {
                foreach (var movieRepositoryValue in movieRepositoryValues)
                {
                    foreach (var showtimeRepositoryValue in showtimeRepositoryValues)
                    {
                        foreach (var guidGeneratorValue in guidGeneratorValues)
                        {
                            if (auditoriumRepositoryValue != null && auditoriumRepositoryValue.Equals(auditoriumRepository) &&
                                    movieRepositoryValue != null && movieRepositoryValue.Equals(movieRepository) &&
                                    showtimeRepositoryValue != null && showtimeRepositoryValue.Equals(showtimeRepository) &&
                                    guidGeneratorValue != null && guidGeneratorValue.Equals(guidGenerator))
                            {
                                continue;
                            }

                            yield return new object[] { auditoriumRepositoryValue, movieRepositoryValue, showtimeRepositoryValue, guidGeneratorValue };
                        }
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateHandleScheduleShowtimeCommand))]
        public void CreateHandleScheduleShowtimeCommand_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            IRepository<Auditorium, AuditoriumId> auditoriumRepository,
            IRepository<Movie, MovieId> movieRepository,
            IRepository<Showtime, ShowtimeId> showtimeRepository,
            IGuidGenerator guidGenerator)
        {
            FluentActions.Invoking(() => new ScheduleShowtimeCommandHandler(auditoriumRepository, movieRepository, showtimeRepository, guidGenerator))
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

            this.mockGuidGenerator.Create().Returns(Guid.NewGuid());

            // Act
            var result = await this.handler.Handle(createShowtimeCommand, default);

            // Assert
            // 1. Validate correct showtime created based on command.
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();
            result.Should().NotBeNull();

            // 2. Some showtime was added to the repository.
            await this.mockShowtimeRepository.Received(1).AddAsync(Arg.Any<Showtime>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleScheduleShowtimeCommand_WhenAuditoriumNotExist_ShouldReturnAError()
        {
            // Arrange
            var createShowtimeCommand = ScheduleShowtimeCommandUtils.CreateCommand();

            this.mockAuditoriumRepository.GetByIdAsync(Arg.Any<AuditoriumId>(), Arg.Any<CancellationToken>())
                .Returns((Auditorium)null);

            // Act
            var result = await this.handler.Handle(createShowtimeCommand, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Value.Should().BeEmpty();
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

            this.mockMovieRepository.GetByIdAsync(Arg.Any<MovieId>(), Arg.Any<CancellationToken>())
                .Returns((Movie)null);

            // Act
            var result = await this.handler.Handle(createShowtimeCommand, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Value.Should().BeEmpty();
            result.Should().NotBeNull();

            await this.mockShowtimeRepository.Received(0).AddAsync(Arg.Any<Showtime>(), Arg.Any<CancellationToken>());
        }
    }
}