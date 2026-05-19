namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.GrantUser
{
    using System;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.GrantUser;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using Xunit;

    public sealed class GrantUserCommandValidatorTests
    {
        private static readonly string[] DefaultRoles = { Constants.Role.Admin };

        private readonly GrantUserCommandValidator validator;

        public GrantUserCommandValidatorTests()
        {
            this.validator = new GrantUserCommandValidator();
        }

        [Fact]
        public void ValidateGrantUserCommand_WhenCommandIsValid_ShouldNotHaveError()
        {
            // Arrange.
            var grantUserCommand = UserCommandUtils.CreateGrantUserCommand();

            // Act.
            var validationResult = this.validator.Validate(grantUserCommand);

            // Assert.
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateGrantUserCommand_WhenUserIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var grantUserCommand = new GrantUserCommand(
                UserId: Guid.Empty,
                TenantId: Guid.NewGuid(),
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid(),
                Roles: DefaultRoles);

            // Act.
            var validationResult = this.validator.Validate(grantUserCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "UserId is required.");
        }

        [Fact]
        public void ValidateGrantUserCommand_WhenTenantIdIsEmpty_ShouldHaveAnError()
        {
            // Arrange.
            var grantUserCommand = new GrantUserCommand(
                UserId: Guid.NewGuid(),
                TenantId: Guid.Empty,
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid(),
                Roles: DefaultRoles);

            // Act.
            var validationResult = this.validator.Validate(grantUserCommand);

            // Assert.
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle();
            validationResult.Errors.Should().ContainSingle(error => error.ErrorMessage == "TenantId is required.");
        }
    }
}