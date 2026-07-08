namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.RemoveUser
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RemoveUser;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using NSubstitute;
    using Xunit;

    public sealed class RemoveUserCommandHandlerTests
    {
        private readonly RemoveUserCommandHandler handler;
        private readonly IRepository<User, UserId> mockUserRepository;

        public RemoveUserCommandHandlerTests()
        {
            this.mockUserRepository = Substitute.For<IRepository<User, UserId>>();
            this.handler = new RemoveUserCommandHandler(this.mockUserRepository);
        }

        [Fact]
        public void CreateRemoveUserCommandHandler_WhenHavingInvalidArguments_ShouldThrowArgumentNullException()
        {
            FluentActions.Invoking(() => new RemoveUserCommandHandler(default!))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleRemoveUserCommand_WhenUserNotExist_ShouldReturnAError()
        {
            // Arrange.
            var removeUserCommand = UserCommandUtils.CreateRemoveUserCommand();

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns((User)null!);

            // Act.
            var result = await this.handler.Handle(removeUserCommand, default);

            // Assert.
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            await this.mockUserRepository.Received(0).DeleteAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleRemoveUserCommand_WhenUserExists_ShouldDeleteAndReturnSuccess()
        {
            // Arrange.
            var removeUserCommand = UserCommandUtils.CreateRemoveUserCommand();
            var existingUser = CreateUserUtils.Create();

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns(existingUser);

            // Act.
            var result = await this.handler.Handle(removeUserCommand, default);

            // Assert.
            result.IsSuccess.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            await this.mockUserRepository.Received(1).DeleteAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }
    }
}