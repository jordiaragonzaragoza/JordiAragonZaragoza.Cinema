namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.CreateUser
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.CreateUser;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using NSubstitute;
    using Xunit;

    public sealed class CreateUserCommandHandlerTests
    {
        private readonly CreateUserCommandHandler handler;
        private readonly IRepository<User, UserId> mockUserRepository;

        public CreateUserCommandHandlerTests()
        {
            this.mockUserRepository = Substitute.For<IRepository<User, UserId>>();
            this.handler = new CreateUserCommandHandler(this.mockUserRepository);
        }

        public static IEnumerable<object[]> InvalidArgumentsCreateUserCommandHandler()
        {
            yield return new object[] { default! };
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateUserCommandHandler))]
        public void CreateUserCommandHandler_WhenHavingInvalidArguments_ShouldThrowArgumentNullException(
            IRepository<User, UserId> userRepository)
        {
            FluentActions.Invoking(() => new CreateUserCommandHandler(userRepository))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleCreateUserCommand_WhenCommandIsValid_ShouldCreateAndReturnSuccess()
        {
            // Arrange.
            var createUserCommand = UserCommandUtils.CreateUserCommand();

            // Act.
            var result = await this.handler.Handle(createUserCommand, default);

            // Assert.
            result.IsSuccess.Should().BeTrue();
            result.Should().NotBeNull();
            await this.mockUserRepository.Received(1).AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }
    }
}