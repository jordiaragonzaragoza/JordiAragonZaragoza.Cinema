namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.GrantUser
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.GrantUser;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using NSubstitute;
    using Xunit;

    public sealed class GrantUserCommandHandlerTests
    {
        private readonly GrantUserCommandHandler handler;
        private readonly IRepository<User, UserId> mockUserRepository;

        public GrantUserCommandHandlerTests()
        {
            this.mockUserRepository = Substitute.For<IRepository<User, UserId>>();
            this.handler = new GrantUserCommandHandler(this.mockUserRepository);
        }

        public static IEnumerable<object[]> InvalidArgumentsCreateGrantUserCommandHandler()
        {
            yield return new object[] { default! };
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateGrantUserCommandHandler))]
        public void CreateGrantUserCommandHandler_WhenHavingInvalidArguments_ShouldThrowArgumentNullException(
            IRepository<User, UserId> userRepository)
        {
            FluentActions.Invoking(() => new GrantUserCommandHandler(userRepository))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleGrantUserCommand_WhenUserNotExist_ShouldReturnAError()
        {
            // Arrange.
            var grantUserCommand = UserCommandUtils.CreateGrantUserCommand();

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns((User)null!);

            // Act.
            var result = await this.handler.Handle(grantUserCommand, default);

            // Assert.
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Should().NotBeNull();
            await this.mockUserRepository.Received(0).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleGrantUserCommand_WhenUserExists_ShouldUpdateAndReturnSuccess()
        {
            // Arrange.
            var grantUserCommand = UserCommandUtils.CreateGrantUserCommand();
            var existingUser = CreateUserUtils.Create();

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns(existingUser);

            // Act.
            var result = await this.handler.Handle(grantUserCommand, default);

            // Assert.
            result.IsSuccess.Should().BeTrue();
            result.Should().NotBeNull();
            await this.mockUserRepository.Received(1).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }
    }
}