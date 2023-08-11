namespace JordiAragon.Cinema.Application.UnitTests.Features.Showtime.Commands.CreateShowtime
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Application.Contracts.Features.Showtime.Commands;
    using JordiAragon.Cinema.Application.Features.Showtime.Commands.CreateShowtime;
    using JordiAragon.Cinema.Application.UnitTests.Features.Showtime.Commands.TestUtils;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using NSubstitute;
    using Xunit;

    public class CreateShowtimeCommandValidatorTests
    {
        private readonly IDateTime mockDatetime;
        private readonly CreateShowtimeCommandValidator validator;

        public CreateShowtimeCommandValidatorTests()
        {
            this.mockDatetime = Substitute.For<IDateTime>();
            this.validator = new CreateShowtimeCommandValidator(this.mockDatetime);
        }

        [Fact]
        public void CreateShowtimeCommandValidator_WhenHavingInvalidArguments_ShouldThrowArgumentException()
        {
            FluentActions.Invoking(() => new CreateShowtimeCommandValidator(null))
            .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ValidateCreateShowtimeCommand_WhenCommandIsValid_ShouldNotHaveError()
        {
            // Arrange.
            var createShowtimeCommand = CreateShowtimeCommandUtils.CreateCommand();

            this.mockDatetime.UtcNow.Returns(DateTime.UtcNow);

            // Act.
            var validationResult = this.validator.Validate(createShowtimeCommand);

            // Assert.
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateCreateShowtimeCommand_WhenMovieIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var createShowtimeCommand = new CreateShowtimeCommand
            {
                AuditoriumId = AuditoriumId.Create(Guid.NewGuid()),
                SessionDateOnUtc = DateTime.UtcNow.AddYears(1),
            };

            this.mockDatetime.UtcNow.Returns(DateTime.UtcNow);

            // Act.
            var validationResult = this.validator.Validate(createShowtimeCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "MovieId is required.");
        }

        [Fact]
        public void ValidateCreateShowtimeCommand_WhenAuditoriumIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var createShowtimeCommand = new CreateShowtimeCommand
            {
                MovieId = MovieId.Create(Guid.NewGuid()),
                SessionDateOnUtc = DateTime.UtcNow.AddYears(1),
            };

            this.mockDatetime.UtcNow.Returns(DateTime.UtcNow);

            // Act.
            var validationResult = this.validator.Validate(createShowtimeCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "AuditoriumId is required.");
        }

        [Fact]
        public void ValidateCreateShowtimeCommand_WhenSessionDateOnUtcExpired_ShouldHaveAnError()
        {
            // Arrange.
            var createShowtimeCommand = new CreateShowtimeCommand
            {
                MovieId = MovieId.Create(Guid.NewGuid()),
                AuditoriumId = AuditoriumId.Create(Guid.NewGuid()),
                SessionDateOnUtc = DateTime.UtcNow.AddYears(-1),
            };

            this.mockDatetime.UtcNow.Returns(DateTime.UtcNow);

            // Act.
            var validationResult = this.validator.Validate(createShowtimeCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "Session Date must be a future date.");
        }
    }
}