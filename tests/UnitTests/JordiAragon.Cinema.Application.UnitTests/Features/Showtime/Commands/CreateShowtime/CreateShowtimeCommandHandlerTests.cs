namespace JordiAragon.Cinema.Application.UnitTests.Features.Showtime.Commands.CreateShowtime
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinema.Application.Features.Showtime.Commands.CreateShowtime;
    using JordiAragon.Cinema.Application.UnitTests.Features.Auditorium.TestUtils;
    using JordiAragon.Cinema.Application.UnitTests.Features.Movie.TestUtils;
    using JordiAragon.Cinema.Application.UnitTests.Features.Showtime.Commands.TestUtils;
    using JordiAragon.Cinema.Application.UnitTests.Features.Showtime.TestUtils;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate.Specifications;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate.Specifications;
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

        // TODO: Add empty constructor tests.
        // T1: System Under Test - Logical component we're testing.
        // T2: Scenario - What we're testing.
        // T3: Expected outcome - What we expect the logical component to do
        [Fact]
        public async Task HandleCreateShowtimeCommand_WhenShowtimeIsValid_ShouldCreateAndReturnShowtimeGuid()
        {
            // Arrange. Given
            // Get hold of a valid showtime
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

            // Act. When.
            // Invoke the handler.
            var result = await this.handler.Handle(createShowtimeCommand, default);

            // Assert. Then.
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
            // Arrange. Given
            var createShowtimeCommand = CreateShowtimeCommandUtils.CreateCommand();
            var existingShowtime = CreateShowtimeUtils.Create();

            this.mockShowtimeRepository.FirstOrDefaultAsync(Arg.Any<ShowtimeByMovieIdSessionDateSpec>(), Arg.Any<CancellationToken>())
                .Returns(existingShowtime);

            // Act. When.
            // Invoke the handler.
            var result = await this.handler.Handle(createShowtimeCommand, default);

            // Assert. Then.
            result.IsSuccess.Should().BeFalse();
            result.ValidationErrors.Should().HaveCount(1);
            result.Value.Should().BeEmpty();
            result.Should().NotBeNull();

            await this.mockShowtimeRepository.Received(0).AddAsync(Arg.Any<Showtime>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleCreateShowtimeCommand_WhenAuditoriumNotExist_ShouldReturnAError()
        {
            // Arrange. Given
            var createShowtimeCommand = CreateShowtimeCommandUtils.CreateCommand();

            this.mockAuditoriumRepository.FirstOrDefaultAsync(Arg.Any<AuditoriumByIdSpec>(), Arg.Any<CancellationToken>())
                .Returns((Auditorium)null);

            // Act. When Invoke the handler.
            var result = await this.handler.Handle(createShowtimeCommand, default);

            // Assert. Then.
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Value.Should().BeEmpty();
            result.Should().NotBeNull();

            await this.mockShowtimeRepository.Received(0).AddAsync(Arg.Any<Showtime>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleCreateShowtimeCommand_WhenMovieNotExist_ShouldReturnAError()
        {
            // Arrange. Given
            var createShowtimeCommand = CreateShowtimeCommandUtils.CreateCommand();

            var existingAuditorium = CreateAuditoriumUtils.Create();

            this.mockShowtimeRepository.FirstOrDefaultAsync(Arg.Any<ShowtimeByMovieIdSessionDateSpec>(), Arg.Any<CancellationToken>())
                .Returns((Showtime)null);

            this.mockAuditoriumRepository.FirstOrDefaultAsync(Arg.Any<AuditoriumByIdSpec>(), Arg.Any<CancellationToken>())
                .Returns(existingAuditorium);

            this.mockMovieRepository.FirstOrDefaultAsync(Arg.Any<MovieByIdSpec>(), Arg.Any<CancellationToken>())
                .Returns((Movie)null);

            // Act. When Invoke the handler.
            var result = await this.handler.Handle(createShowtimeCommand, default);

            // Assert. Then.
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Value.Should().BeEmpty();
            result.Should().NotBeNull();

            await this.mockShowtimeRepository.Received(0).AddAsync(Arg.Any<Showtime>(), Arg.Any<CancellationToken>());
        }
    }
}