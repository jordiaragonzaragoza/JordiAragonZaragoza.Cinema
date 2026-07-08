namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.AssignRole
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
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.AssignRole;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using NSubstitute;
    using Xunit;

    public sealed class AssignRoleCommandHandlerTests
    {
        private readonly AssignRoleCommandHandler handler;
        private readonly IRepository<User, UserId> mockUserRepository;

        public AssignRoleCommandHandlerTests()
        {
            this.mockUserRepository = Substitute.For<IRepository<User, UserId>>();
            this.handler = new AssignRoleCommandHandler(this.mockUserRepository);
        }

        public static IEnumerable<object[]> InvalidArgumentsCreateAssignRoleCommandHandler()
        {
            yield return new object[] { default! };
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateAssignRoleCommandHandler))]
        public void CreateAssignRoleCommandHandler_WhenHavingInvalidArguments_ShouldThrowArgumentNullException(
            IRepository<User, UserId> userRepository)
        {
            FluentActions.Invoking(() => new AssignRoleCommandHandler(userRepository))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleAssignRoleCommand_WhenUserNotExist_ShouldReturnAError()
        {
            // Arrange.
            var assignRoleCommand = UserCommandUtils.CreateAssignRoleCommand();

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns((User)null!);

            // Act.
            var result = await this.handler.Handle(assignRoleCommand, default);

            // Assert.
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Should().NotBeNull();
            await this.mockUserRepository.Received(0).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleAssignRoleCommand_WhenUserExists_ShouldUpdateAndReturnSuccess()
        {
            // Arrange.
            var assignRoleCommand = UserCommandUtils.CreateAssignRoleCommand();
            var existingUser = CreateUserUtils.Create();
            var scope = Scope.Create(
                new TenantId(assignRoleCommand.TenantId),
                new PartitionId(assignRoleCommand.PartitionId!.Value),
                new CinemaId(assignRoleCommand.CinemaId!.Value));

            existingUser.GrantUser(scope, new List<Role> { Constants.Role.Admin }, []);

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns(existingUser);

            // Act.
            var result = await this.handler.Handle(assignRoleCommand, default);

            // Assert.
            result.IsSuccess.Should().BeTrue();
            result.Should().NotBeNull();
            await this.mockUserRepository.Received(1).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }
    }
}