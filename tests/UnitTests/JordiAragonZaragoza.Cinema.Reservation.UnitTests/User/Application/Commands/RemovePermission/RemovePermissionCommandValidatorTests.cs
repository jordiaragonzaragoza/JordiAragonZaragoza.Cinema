namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.RemovePermission
{
    using System;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RemovePermission;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using Xunit;

    public sealed class RemovePermissionCommandValidatorTests
    {
        private readonly RemovePermissionCommandValidator validator;

        public RemovePermissionCommandValidatorTests()
        {
            this.validator = new RemovePermissionCommandValidator();
        }

        [Fact]
        public void ValidateRemovePermissionCommand_WhenCommandIsValid_ShouldNotHaveError()
        {
            // Arrange.
            var removePermissionCommand = UserCommandUtils.CreateRemovePermissionCommand();

            // Act.
            var validationResult = this.validator.Validate(removePermissionCommand);

            // Assert.
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateRemovePermissionCommand_WhenUserIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var removePermissionCommand = new RemovePermissionCommand(
                UserId: Guid.Empty,
                TenantId: Guid.NewGuid(),
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid(),
                Permission: ShowtimePermisions.GetShowtimes);

            // Act.
            var validationResult = this.validator.Validate(removePermissionCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "UserId is required.");
        }

        [Fact]
        public void ValidateRemovePermissionCommand_WhenTenantIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var removePermissionCommand = new RemovePermissionCommand(
                UserId: Guid.NewGuid(),
                TenantId: Guid.Empty,
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid(),
                Permission: ShowtimePermisions.GetShowtimes);

            // Act.
            var validationResult = this.validator.Validate(removePermissionCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "TenantId is required.");
        }

        [Fact]
        public void ValidateRemovePermissionCommand_WhenPermissionIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var removePermissionCommand = new RemovePermissionCommand(
                UserId: Guid.NewGuid(),
                TenantId: Guid.NewGuid(),
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid(),
                Permission: string.Empty);

            // Act.
            var validationResult = this.validator.Validate(removePermissionCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "Permission is required.");
        }
    }
}
