namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.RevokeUser
{
    using System;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RevokeUser;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using Xunit;

    public sealed class RevokeUserCommandValidatorTests
    {
        private readonly RevokeUserCommandValidator validator;

        public RevokeUserCommandValidatorTests()
        {
            this.validator = new RevokeUserCommandValidator();
        }

        [Fact]
        public void ValidateRevokeUserCommand_WhenCommandIsValid_ShouldNotHaveError()
        {
            // Arrange.
            var revokeUserCommand = UserCommandUtils.CreateRevokeUserCommand();

            // Act.
            var validationResult = this.validator.Validate(revokeUserCommand);

            // Assert.
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateRevokeUserCommand_WhenUserIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var revokeUserCommand = new RevokeUserCommand(
                UserId: Guid.Empty,
                TenantId: Guid.CreateVersion7(),
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7());

            // Act.
            var validationResult = this.validator.Validate(revokeUserCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "UserId is required.");
        }

        [Fact]
        public void ValidateRevokeUserCommand_WhenTenantIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var revokeUserCommand = new RevokeUserCommand(
                UserId: Guid.CreateVersion7(),
                TenantId: Guid.Empty,
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7());

            // Act.
            var validationResult = this.validator.Validate(revokeUserCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "TenantId is required.");
        }
    }
}