namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.AssignPermission
{
    using System;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.AssignPermission;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using Xunit;

    public sealed class AssignPermissionCommandValidatorTests
    {
        private readonly AssignPermissionCommandValidator validator;

        public AssignPermissionCommandValidatorTests()
        {
            this.validator = new AssignPermissionCommandValidator();
        }

        [Fact]
        public void ValidateAssignPermissionCommand_WhenCommandIsValid_ShouldNotHaveError()
        {
            // Arrange.
            var assignPermissionCommand = UserCommandUtils.CreateAssignPermissionCommand();

            // Act.
            var validationResult = this.validator.Validate(assignPermissionCommand);

            // Assert.
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateAssignPermissionCommand_WhenUserIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var assignPermissionCommand = new AssignPermissionCommand(
                UserId: Guid.Empty,
                TenantId: Guid.CreateVersion7(),
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7(),
                Permission: ShowtimePermisions.ScheduleShowtime);

            // Act.
            var validationResult = this.validator.Validate(assignPermissionCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "UserId is required.");
        }

        [Fact]
        public void ValidateAssignPermissionCommand_WhenTenantIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var assignPermissionCommand = new AssignPermissionCommand(
                UserId: Guid.CreateVersion7(),
                TenantId: Guid.Empty,
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7(),
                Permission: ShowtimePermisions.ScheduleShowtime);

            // Act.
            var validationResult = this.validator.Validate(assignPermissionCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "TenantId is required.");
        }

        [Fact]
        public void ValidateAssignPermissionCommand_WhenPermissionIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var assignPermissionCommand = new AssignPermissionCommand(
                UserId: Guid.CreateVersion7(),
                TenantId: Guid.CreateVersion7(),
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7(),
                Permission: string.Empty);

            // Act.
            var validationResult = this.validator.Validate(assignPermissionCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "Permission is required.");
        }
    }
}
