namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.RevokeUser
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RevokeUser;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using NSubstitute;
    using Xunit;

    public sealed class RevokeUserCommandHandlerTests
    {
        private readonly RevokeUserCommandHandler handler;
        private readonly IRepository<User, UserId> mockUserRepository;

        public RevokeUserCommandHandlerTests()
        {
            this.mockUserRepository = Substitute.For<IRepository<User, UserId>>();
            this.handler = new RevokeUserCommandHandler(this.mockUserRepository);
        }

        [Fact]
        public void CreateRevokeUserCommandHandler_WhenHavingInvalidArguments_ShouldThrowArgumentNullException()
        {
            FluentActions.Invoking(() => new RevokeUserCommandHandler(default!))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleRevokeUserCommand_WhenUserNotExist_ShouldReturnAError()
        {
            // Arrange.
            var revokeUserCommand = UserCommandUtils.CreateRevokeUserCommand();

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns((User)null!);

            // Act.
            var result = await this.handler.Handle(revokeUserCommand, default);

            // Assert.
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            await this.mockUserRepository.Received(0).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleRevokeUserCommand_WhenUserExists_ShouldUpdateAndReturnSuccess()
        {
            // Arrange.
            var revokeUserCommand = UserCommandUtils.CreateRevokeUserCommand();
            var existingUser = CreateUserUtils.Create();
            var scope = Scope.Create(
                new TenantId(revokeUserCommand.TenantId),
                new PartitionId(revokeUserCommand.PartitionId!.Value),
                new CinemaId(revokeUserCommand.CinemaId!.Value));

            existingUser.GrantUser(scope, new List<Role> { Role.Create("Admin") });

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns(existingUser);

            // Act.
            var result = await this.handler.Handle(revokeUserCommand, default);

            // Assert.
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            await this.mockUserRepository.Received(1).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }
    }
}