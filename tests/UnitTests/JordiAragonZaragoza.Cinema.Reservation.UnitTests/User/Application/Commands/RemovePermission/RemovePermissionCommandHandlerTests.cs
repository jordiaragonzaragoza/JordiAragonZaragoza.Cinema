namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Application.Commands.RemovePermission
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
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RemovePermission;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using NSubstitute;
    using Xunit;

    public sealed class RemovePermissionCommandHandlerTests
    {
        private readonly RemovePermissionCommandHandler handler;
        private readonly IRepository<User, UserId> mockUserRepository;

        public RemovePermissionCommandHandlerTests()
        {
            this.mockUserRepository = Substitute.For<IRepository<User, UserId>>();
            this.handler = new RemovePermissionCommandHandler(this.mockUserRepository);
        }

        public static IEnumerable<object[]> InvalidArgumentsCreateRemovePermissionCommandHandler()
        {
            yield return new object[] { default! };
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateRemovePermissionCommandHandler))]
        public void CreateRemovePermissionCommandHandler_WhenHavingInvalidArguments_ShouldThrowArgumentNullException(
            IRepository<User, UserId> userRepository)
        {
            FluentActions.Invoking(() => new RemovePermissionCommandHandler(userRepository))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task HandleRemovePermissionCommand_WhenUserNotExist_ShouldReturnAnError()
        {
            // Arrange.
            var removePermissionCommand = UserCommandUtils.CreateRemovePermissionCommand();

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns((User)null!);

            // Act.
            var result = await this.handler.Handle(removePermissionCommand, default);

            // Assert.
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Should().NotBeNull();
            await this.mockUserRepository.Received(0).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleRemovePermissionCommand_WhenUserExists_ShouldUpdateAndReturnSuccess()
        {
            // Arrange.
            var removePermissionCommand = UserCommandUtils.CreateRemovePermissionCommand();
            var existingUser = CreateUserUtils.Create();
            var scope = Scope.Create(
                new TenantId(removePermissionCommand.TenantId),
                new PartitionId(removePermissionCommand.PartitionId!.Value),
                new CinemaId(removePermissionCommand.CinemaId!.Value));

            var permissions = new List<Permission>
            {
                Permission.Create(ShowtimePermisions.GetShowtimes),
                Permission.Create(ShowtimePermisions.ScheduleShowtime),
            };

            existingUser.GrantUser(scope, [], permissions);

            this.mockUserRepository.GetByIdAsync(Arg.Any<UserId>(), Arg.Any<CancellationToken>())
                .Returns(existingUser);

            // Act.
            var result = await this.handler.Handle(removePermissionCommand, default);

            // Assert.
            result.IsSuccess.Should().BeTrue();
            result.Should().NotBeNull();
            await this.mockUserRepository.Received(1).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }
    }
}
