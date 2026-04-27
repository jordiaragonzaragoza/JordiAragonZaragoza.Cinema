namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.AssignRole
{
    using System;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.AssignRole;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using Xunit;

    public sealed class AssignRoleCommandValidatorTests
    {
        private readonly AssignRoleCommandValidator validator;

        public AssignRoleCommandValidatorTests()
        {
            this.validator = new AssignRoleCommandValidator();
        }

        [Fact]
        public void ValidateAssignRoleCommand_WhenCommandIsValid_ShouldNotHaveError()
        {
            // Arrange.
            var assignRoleCommand = UserCommandUtils.CreateAssignRoleCommand();

            // Act.
            var validationResult = this.validator.Validate(assignRoleCommand);

            // Assert.
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateAssignRoleCommand_WhenUserIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var assignRoleCommand = new AssignRoleCommand(
                UserId: Guid.Empty,
                TenantId: Guid.NewGuid(),
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid(),
                Role: "Viewer");

            // Act.
            var validationResult = this.validator.Validate(assignRoleCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "UserId is required.");
        }

        [Fact]
        public void ValidateAssignRoleCommand_WhenTenantIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var assignRoleCommand = new AssignRoleCommand(
                UserId: Guid.NewGuid(),
                TenantId: Guid.Empty,
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid(),
                Role: "Viewer");

            // Act.
            var validationResult = this.validator.Validate(assignRoleCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "TenantId is required.");
        }

        [Fact]
        public void ValidateAssignRoleCommand_WhenRoleIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var assignRoleCommand = new AssignRoleCommand(
                UserId: Guid.NewGuid(),
                TenantId: Guid.NewGuid(),
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid(),
                Role: string.Empty);

            // Act.
            var validationResult = this.validator.Validate(assignRoleCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "Role is required.");
        }
    }
}