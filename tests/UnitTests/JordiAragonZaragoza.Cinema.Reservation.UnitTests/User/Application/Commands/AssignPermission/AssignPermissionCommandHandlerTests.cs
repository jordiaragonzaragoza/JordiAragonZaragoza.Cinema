namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.AssignPermission
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.AssignPermission;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using NSubstitute;
    using Xunit;

    public sealed class AssignPermissionCommandHandlerTests
    {
        private readonly AssignPermissionCommandHandler handler;
        private readonly IRepository<User, UserId> mockUserRepository;

        public AssignPermissionCommandHandlerTests()
        {
            this.mockUserRepository = Substitute.For<IRepository<User, UserId>>();
            this.handler = new AssignPermissionCommandHandler(this.mockUserRepository);
        }

        public static IEnumerable<object[]> InvalidArgumentsCreateAssignPermissionCommandHandler()
        {
            yield return new object[] { default! };
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateAssignPermissionCommandHandler))]
        public void CreateAssignPermissionCommandHandler_WhenHavingInvalidArguments_ShouldThrowArgumentNullException(
            IRepository<User, UserId> userRepository)
        {
            FluentActions.Invoking(() => new AssignPermissionCommandHandler(userRepository))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleAssignPermissionCommand_WhenUserNotExist_ShouldReturnAnError()
        {
            // Arrange.
            var assignPermissionCommand = UserCommandUtils.CreateAssignPermissionCommand();

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns((User)null!);

            // Act.
            var result = await this.handler.Handle(assignPermissionCommand, default);

            // Assert.
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Should().NotBeNull();
            await this.mockUserRepository.Received(0).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleAssignPermissionCommand_WhenUserExists_ShouldUpdateAndReturnSuccess()
        {
            // Arrange.
            var assignPermissionCommand = UserCommandUtils.CreateAssignPermissionCommand();
            var existingUser = CreateUserUtils.Create();
            var scope = Scope.Create(
                new TenantId(assignPermissionCommand.TenantId),
                new PartitionId(assignPermissionCommand.PartitionId!.Value),
                new CinemaId(assignPermissionCommand.CinemaId!.Value));

            existingUser.GrantUser(scope, [], new List<Permission> { Permission.Create(ShowtimePermisions.GetShowtimes) });

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns(existingUser);

            // Act.
            var result = await this.handler.Handle(assignPermissionCommand, default);

            // Assert.
            result.IsSuccess.Should().BeTrue();
            result.Should().NotBeNull();
            await this.mockUserRepository.Received(1).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }
    }
}
