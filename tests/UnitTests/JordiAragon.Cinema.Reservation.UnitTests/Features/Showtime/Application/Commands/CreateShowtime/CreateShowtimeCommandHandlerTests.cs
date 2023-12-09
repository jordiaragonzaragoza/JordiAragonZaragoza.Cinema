namespace JordiAragon.Cinema.Reservation.UnitTests.Showtime.Application.Commands.CreateShowtime
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Commands.CreateShowtime;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Specifications;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Application;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using NSubstitute;
    using Volo.Abp.Guids;
    using Xunit;

    public class CreateShowtimeCommandHandlerTests
    {
        private readonly CreateShowtimeCommandHandler handler;

        private readonly IRepository<Auditorium, AuditoriumId> mockAuditoriumRepository;
        private readonly IRepository<Movie, MovieId> mockMovieRepository;
        private readonly IRepository<Showtime, ShowtimeId> mockShowtimeRepository;
        private readonly ISpecificationReadRepository<Showtime, ShowtimeId> mockShowtimeReadRepository;
        private readonly IGuidGenerator mockGuidGenerator;

        public CreateShowtimeCommandHandlerTests()
        {
            this.mockAuditoriumRepository = Substitute.For<IRepository<Auditorium, AuditoriumId>>();
            this.mockMovieRepository = Substitute.For<IRepository<Movie, MovieId>>();
            this.mockShowtimeRepository = Substitute.For<IRepository<Showtime, ShowtimeId>>();
            this.mockShowtimeReadRepository = Substitute.For<ISpecificationReadRepository<Showtime, ShowtimeId>>();
            this.mockGuidGenerator = Substitute.For<IGuidGenerator>();

            this.handler = new CreateShowtimeCommandHandler(
                this.mockAuditoriumRepository,
                this.mockMovieRepository,
                this.mockShowtimeRepository,
                this.mockShowtimeReadRepository,
                this.mockGuidGenerator);
        }

        public static IEnumerable<object[]> InvalidArgumentsCreateHandleCreateShowtimeCommand()
        {
            var auditoriumRepository = Substitute.For<IReadRepository<Auditorium, AuditoriumId>>();
            var movieRepository = Substitute.For<IReadRepository<Movie, MovieId>>();
            var showtimeRepository = Substitute.For<IRepository<Showtime, ShowtimeId>>();
            var showtimeReadRepository = Substitute.For<ISpecificationReadRepository<Showtime, ShowtimeId>>();
            var guidGenerator = Substitute.For<IGuidGenerator>();

            var auditoriumRepositoryValues = new object[] { null, auditoriumRepository };
            var movieRepositoryValues = new object[] { null, movieRepository };
            var showtimeRepositoryValues = new object[] { null, showtimeRepository };
            var showtimeReadRepositoryValues = new object[] { null, showtimeReadRepository };
            var guidGeneratorValues = new object[] { null, guidGenerator };

            foreach (var auditoriumRepositoryValue in auditoriumRepositoryValues)
            {
                foreach (var movieRepositoryValue in movieRepositoryValues)
                {
                    foreach (var showtimeRepositoryValue in showtimeRepositoryValues)
                    {
                        foreach (var showtimeReadRepositoryValue in showtimeReadRepositoryValues)
                        {
                            foreach (var guidGeneratorValue in guidGeneratorValues)
                            {
                                if (auditoriumRepositoryValue != null && auditoriumRepositoryValue.Equals(auditoriumRepository) &&
                                        movieRepositoryValue != null && movieRepositoryValue.Equals(movieRepository) &&
                                        showtimeRepositoryValue != null && showtimeRepositoryValue.Equals(showtimeRepository) &&
                                        showtimeReadRepositoryValue != null && showtimeReadRepositoryValue.Equals(showtimeReadRepository) &&
                                        guidGeneratorValue != null && guidGeneratorValue.Equals(guidGenerator))
                                {
                                    continue;
                                }

                                yield return new object[] { auditoriumRepositoryValue, movieRepositoryValue, showtimeRepositoryValue, showtimeReadRepositoryValue, guidGeneratorValue };
                            }
                        }
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateHandleCreateShowtimeCommand))]
        public void CreateHandleCreateShowtimeCommand_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            IRepository<Auditorium, AuditoriumId> auditoriumRepository,
            IRepository<Movie, MovieId> movieRepository,
            IRepository<Showtime, ShowtimeId> showtimeRepository,
            ISpecificationReadRepository<Showtime, ShowtimeId> showtimeReadRepository,
            IGuidGenerator guidGenerator)
        {
            FluentActions.Invoking(() => new CreateShowtimeCommandHandler(auditoriumRepository, movieRepository, showtimeRepository, showtimeReadRepository, guidGenerator))
            .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleCreateShowtimeCommand_WhenShowtimeIsValid_ShouldCreateAndReturnShowtimeGuid()
        {
            // Arrange
            var createShowtimeCommand = CreateShowtimeCommandUtils.CreateCommand();

            var existingMovie = CreateMovieUtils.Create();
            var existingAuditorium = CreateAuditoriumUtils.Create();

            this.mockShowtimeReadRepository.FirstOrDefaultAsync(Arg.Any<ShowtimeByMovieIdSessionDateSpec>(), Arg.Any<CancellationToken>())
                .Returns((Showtime)null);

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
        public async Task HandleCreateShowtimeCommand_WhenShowtimeExist_ShouldReturnAValidationError()
        {
            // Arrange
            var createShowtimeCommand = CreateShowtimeCommandUtils.CreateCommand();
            var existingShowtime = CreateShowtimeUtils.Create();

            this.mockShowtimeReadRepository.FirstOrDefaultAsync(Arg.Any<ShowtimeByMovieIdSessionDateSpec>(), Arg.Any<CancellationToken>())
                .Returns(existingShowtime);

            // Act
            var result = await this.handler.Handle(createShowtimeCommand, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ValidationErrors.Should().HaveCount(1);
            result.Value.Should().BeEmpty();
            result.Should().NotBeNull();

            await this.mockShowtimeRepository.Received(0).AddAsync(Arg.Any<Showtime>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleCreateShowtimeCommand_WhenAuditoriumNotExist_ShouldReturnAError()
        {
            // Arrange
            var createShowtimeCommand = CreateShowtimeCommandUtils.CreateCommand();

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
        public async Task HandleCreateShowtimeCommand_WhenMovieNotExist_ShouldReturnAError()
        {
            // Arrange
            var createShowtimeCommand = CreateShowtimeCommandUtils.CreateCommand();

            var existingAuditorium = CreateAuditoriumUtils.Create();

            this.mockShowtimeReadRepository.FirstOrDefaultAsync(Arg.Any<ShowtimeByMovieIdSessionDateSpec>(), Arg.Any<CancellationToken>())
                .Returns((Showtime)null);

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