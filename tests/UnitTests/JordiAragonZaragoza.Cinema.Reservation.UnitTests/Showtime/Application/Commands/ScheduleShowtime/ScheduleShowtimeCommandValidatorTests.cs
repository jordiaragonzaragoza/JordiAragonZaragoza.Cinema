namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Showtime.Application.Commands.ScheduleShowtime
{
    using System;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.ScheduleShowtime;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using NSubstitute;
    using Xunit;

    public sealed class ScheduleShowtimeCommandValidatorTests
    {
        private readonly IDateTime mockDatetime;
        private readonly ScheduleShowtimeCommandValidator validator;

        public ScheduleShowtimeCommandValidatorTests()
        {
            this.mockDatetime = Substitute.For<IDateTime>();
            this.validator = new ScheduleShowtimeCommandValidator(this.mockDatetime);
        }

        [Fact]
        public void ScheduleShowtimeCommandValidator_WhenHavingInvalidArguments_ShouldThrowArgumentException()
        {
            IDateTime dateTime = null!;

            FluentActions.Invoking(() => new ScheduleShowtimeCommandValidator(dateTime))
            .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ValidateScheduleShowtimeCommand_WhenCommandIsValid_ShouldNotHaveError()
        {
            // Arrange.
            var createShowtimeCommand = ScheduleShowtimeCommandUtils.CreateCommand();

            this.mockDatetime.UtcNow.Returns(DateTimeOffset.UtcNow);

            // Act.
            var validationResult = this.validator.Validate(createShowtimeCommand);

            // Assert.
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateScheduleShowtimeCommand_WhenMovieIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var createShowtimeCommand = new ScheduleShowtimeCommand(
                AuditoriumId: AuditoriumId.Create(Guid.NewGuid()),
                MovieId: Guid.Empty,
                SessionDateOnUtc: DateTimeOffset.UtcNow.AddYears(1));

            this.mockDatetime.UtcNow.Returns(DateTimeOffset.UtcNow);

            // Act.
            var validationResult = this.validator.Validate(createShowtimeCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "MovieId is required.");
        }

        [Fact]
        public void ValidateScheduleShowtimeCommand_WhenAuditoriumIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var createShowtimeCommand = new ScheduleShowtimeCommand(
                AuditoriumId: Guid.Empty,
                MovieId: MovieId.Create(Guid.NewGuid()),
                SessionDateOnUtc: DateTimeOffset.UtcNow.AddYears(1));

            this.mockDatetime.UtcNow.Returns(DateTimeOffset.UtcNow);

            // Act.
            var validationResult = this.validator.Validate(createShowtimeCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "AuditoriumId is required.");
        }

        [Fact]
        public void ValidateScheduleShowtimeCommand_WhenSessionDateOnUtcExpired_ShouldHaveAnError()
        {
            // Arrange.
            var createShowtimeCommand = new ScheduleShowtimeCommand(
                AuditoriumId.Create(Guid.NewGuid()),
                MovieId.Create(Guid.NewGuid()),
                DateTimeOffset.UtcNow.AddYears(-1));

            this.mockDatetime.UtcNow.Returns(DateTimeOffset.UtcNow);

            // Act.
            var validationResult = this.validator.Validate(createShowtimeCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "Session Date must be a future date.");
        }
    }
}