namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.RemoveRole
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
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RemoveRole;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using NSubstitute;
    using Xunit;

    public sealed class RemoveRoleCommandHandlerTests
    {
        private readonly RemoveRoleCommandHandler handler;
        private readonly IRepository<User, UserId> mockUserRepository;

        public RemoveRoleCommandHandlerTests()
        {
            this.mockUserRepository = Substitute.For<IRepository<User, UserId>>();
            this.handler = new RemoveRoleCommandHandler(this.mockUserRepository);
        }

        [Fact]
        public void CreateRemoveRoleCommandHandler_WhenHavingInvalidArguments_ShouldThrowArgumentNullException()
        {
            FluentActions.Invoking(() => new RemoveRoleCommandHandler(default!))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleRemoveRoleCommand_WhenUserNotExist_ShouldReturnAError()
        {
            // Arrange.
            var removeRoleCommand = UserCommandUtils.CreateRemoveRoleCommand();

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns((User)null!);

            // Act.
            var result = await this.handler.Handle(removeRoleCommand, default);

            // Assert.
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            await this.mockUserRepository.Received(0).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleRemoveRoleCommand_WhenUserExists_ShouldUpdateAndReturnSuccess()
        {
            // Arrange.
            var removeRoleCommand = UserCommandUtils.CreateRemoveRoleCommand();
            var existingUser = CreateUserUtils.Create();
            var scope = Scope.Create(
                new TenantId(removeRoleCommand.TenantId),
                new PartitionId(removeRoleCommand.PartitionId!.Value),
                new CinemaId(removeRoleCommand.CinemaId!.Value));

            existingUser.GrantUser(scope, new List<Role> { Role.Create(removeRoleCommand.Role) });

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns(existingUser);

            // Act.
            var result = await this.handler.Handle(removeRoleCommand, default);

            // Assert.
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            await this.mockUserRepository.Received(1).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }
    }
}