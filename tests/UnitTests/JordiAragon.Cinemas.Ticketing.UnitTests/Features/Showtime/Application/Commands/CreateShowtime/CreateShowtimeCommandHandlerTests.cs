﻿namespace JordiAragon.Cinemas.Ticketing.UnitTests.Showtime.Application.Commands.CreateShowtime
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Auditorium.Domain;
    using JordiAragon.Cinemas.Ticketing.Auditorium.Domain.Specifications;
    using JordiAragon.Cinemas.Ticketing.Movie.Domain;
    using JordiAragon.Cinemas.Ticketing.Movie.Domain.Specifications;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Commands.CreateShowtime;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain.Specifications;
    using JordiAragon.Cinemas.Ticketing.UnitTests.TestUtils.Application;
    using JordiAragon.Cinemas.Ticketing.UnitTests.TestUtils.Domain;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using NSubstitute;
    using Volo.Abp.Guids;
    using Xunit;

    public class CreateShowtimeCommandHandlerTests
    {
        private readonly CreateShowtimeCommandHandler handler;

        private readonly IReadRepository<Auditorium> mockAuditoriumRepository;
        private readonly IReadRepository<Movie> mockMovieRepository;
        private readonly IRepository<Showtime> mockShowtimeRepository;
        private readonly IGuidGenerator mockGuidGenerator;

        public CreateShowtimeCommandHandlerTests()
        {
            this.mockAuditoriumRepository = Substitute.For<IReadRepository<Auditorium>>();
            this.mockMovieRepository = Substitute.For<IReadRepository<Movie>>();
            this.mockShowtimeRepository = Substitute.For<IRepository<Showtime>>();
            this.mockGuidGenerator = Substitute.For<IGuidGenerator>();

            this.handler = new CreateShowtimeCommandHandler(
                this.mockAuditoriumRepository,
                this.mockMovieRepository,
                this.mockShowtimeRepository,
                this.mockGuidGenerator);
        }

        public static IEnumerable<object[]> InvalidArgumentsCreateHandleCreateShowtimeCommand()
        {
            var auditoriumRepository = Substitute.For<IReadRepository<Auditorium>>();
            var movieRepository = Substitute.For<IReadRepository<Movie>>();
            var showtimeRepository = Substitute.For<IRepository<Showtime>>();
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
        [MemberData(nameof(InvalidArgumentsCreateHandleCreateShowtimeCommand))]
        public void CreateHandleCreateShowtimeCommand_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            IReadRepository<Auditorium> auditoriumRepository,
            IReadRepository<Movie> movieRepository,
            IRepository<Showtime> showtimeRepository,
            IGuidGenerator guidGenerator)
        {
            FluentActions.Invoking(() => new CreateShowtimeCommandHandler(auditoriumRepository, movieRepository, showtimeRepository, guidGenerator))
            .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleCreateShowtimeCommand_WhenShowtimeIsValid_ShouldCreateAndReturnShowtimeGuid()
        {
            // Arrange
            var createShowtimeCommand = CreateShowtimeCommandUtils.CreateCommand();

            var existingMovie = CreateMovieUtils.Create();
            var existingAuditorium = CreateAuditoriumUtils.Create();

            this.mockShowtimeRepository.FirstOrDefaultAsync(Arg.Any<ShowtimeByMovieIdSessionDateSpec>(), Arg.Any<CancellationToken>())
                .Returns((Showtime)null);

            this.mockAuditoriumRepository.FirstOrDefaultAsync(Arg.Any<AuditoriumByIdSpec>(), Arg.Any<CancellationToken>())
                .Returns(existingAuditorium);

            this.mockMovieRepository.FirstOrDefaultAsync(Arg.Any<MovieByIdSpec>(), Arg.Any<CancellationToken>())
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

            this.mockShowtimeRepository.FirstOrDefaultAsync(Arg.Any<ShowtimeByMovieIdSessionDateSpec>(), Arg.Any<CancellationToken>())
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

            this.mockAuditoriumRepository.FirstOrDefaultAsync(Arg.Any<AuditoriumByIdSpec>(), Arg.Any<CancellationToken>())
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

            this.mockShowtimeRepository.FirstOrDefaultAsync(Arg.Any<ShowtimeByMovieIdSessionDateSpec>(), Arg.Any<CancellationToken>())
                .Returns((Showtime)null);

            this.mockAuditoriumRepository.FirstOrDefaultAsync(Arg.Any<AuditoriumByIdSpec>(), Arg.Any<CancellationToken>())
                .Returns(existingAuditorium);

            this.mockMovieRepository.FirstOrDefaultAsync(Arg.Any<MovieByIdSpec>(), Arg.Any<CancellationToken>())
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