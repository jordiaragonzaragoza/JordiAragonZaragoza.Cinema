namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.RemoveUser
{
    using System;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RemoveUser;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using Xunit;

    public sealed class RemoveUserCommandValidatorTests
    {
        private readonly RemoveUserCommandValidator validator;

        public RemoveUserCommandValidatorTests()
        {
            this.validator = new RemoveUserCommandValidator();
        }

        [Fact]
        public void ValidateRemoveUserCommand_WhenCommandIsValid_ShouldNotHaveError()
        {
            // Arrange.
            var removeUserCommand = UserCommandUtils.CreateRemoveUserCommand();

            // Act.
            var validationResult = this.validator.Validate(removeUserCommand);

            // Assert.
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateRemoveUserCommand_WhenUserIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var removeUserCommand = new RemoveUserCommand(Guid.Empty);

            // Act.
            var validationResult = this.validator.Validate(removeUserCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "UserId is required.");
        }
    }
}