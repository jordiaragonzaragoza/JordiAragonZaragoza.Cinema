namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.CreateUser
{
    using System;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.AddUser;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using Xunit;

    public sealed class CreateUserCommandValidatorTests
    {
        private readonly CreateUserCommandValidator validator;

        public CreateUserCommandValidatorTests()
        {
            this.validator = new CreateUserCommandValidator();
        }

        [Fact]
        public void ValidateCreateUserCommand_WhenCommandIsValid_ShouldNotHaveError()
        {
            // Arrange.
            var createUserCommand = UserCommandUtils.CreateUserCommand();

            // Act.
            var validationResult = this.validator.Validate(createUserCommand);

            // Assert.
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateCreateUserCommand_WhenUserIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var createUserCommand = new CreateUserCommand(Guid.Empty);

            // Act.
            var validationResult = this.validator.Validate(createUserCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "UserId is required.");
        }
    }
}