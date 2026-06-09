namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.RemoveRole
{
    using System;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RemoveRole;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using Xunit;

    public sealed class RemoveRoleCommandValidatorTests
    {
        private readonly RemoveRoleCommandValidator validator;

        public RemoveRoleCommandValidatorTests()
        {
            this.validator = new RemoveRoleCommandValidator();
        }

        [Fact]
        public void ValidateRemoveRoleCommand_WhenCommandIsValid_ShouldNotHaveError()
        {
            // Arrange.
            var removeRoleCommand = UserCommandUtils.CreateRemoveRoleCommand();

            // Act.
            var validationResult = this.validator.Validate(removeRoleCommand);

            // Assert.
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateRemoveRoleCommand_WhenUserIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var removeRoleCommand = new RemoveRoleCommand(
                UserId: Guid.Empty,
                TenantId: Guid.CreateVersion7(),
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7(),
                Role: Constants.Role.Viewer);

            // Act.
            var validationResult = this.validator.Validate(removeRoleCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "UserId is required.");
        }

        [Fact]
        public void ValidateRemoveRoleCommand_WhenTenantIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var removeRoleCommand = new RemoveRoleCommand(
                UserId: Guid.CreateVersion7(),
                TenantId: Guid.Empty,
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7(),
                Role: Constants.Role.Viewer);

            // Act.
            var validationResult = this.validator.Validate(removeRoleCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "TenantId is required.");
        }

        [Fact]
        public void ValidateRemoveRoleCommand_WhenRoleIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var removeRoleCommand = new RemoveRoleCommand(
                UserId: Guid.CreateVersion7(),
                TenantId: Guid.CreateVersion7(),
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7(),
                Role: string.Empty);

            // Act.
            var validationResult = this.validator.Validate(removeRoleCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "Role is required.");
        }
    }
}