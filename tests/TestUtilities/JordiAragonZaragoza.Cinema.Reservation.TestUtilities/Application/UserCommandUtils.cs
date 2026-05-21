namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;

    public static class UserCommandUtils
    {
        private static readonly string[] DefaultRoles = { Constants.Role.Admin };

        private static readonly string[] Permissions = [];

        public static CreateUserCommand CreateUserCommand()
            => new(Guid.NewGuid());

        public static GrantUserCommand CreateGrantUserCommand()
            => new(
                UserId: Guid.NewGuid(),
                TenantId: Guid.NewGuid(),
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid(),
                Roles: DefaultRoles,
                Permissions: Permissions);

        public static AssignRoleCommand CreateAssignRoleCommand()
            => new(
                UserId: Guid.NewGuid(),
                TenantId: Guid.NewGuid(),
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid(),
                Role: Constants.Role.Viewer);

        public static RemoveRoleCommand CreateRemoveRoleCommand()
            => new(
                UserId: Guid.NewGuid(),
                TenantId: Guid.NewGuid(),
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid(),
                Role: Constants.Role.Viewer);

        public static RemoveUserCommand CreateRemoveUserCommand()
            => new(Guid.NewGuid());

        public static RevokeUserCommand CreateRevokeUserCommand()
            => new(
                UserId: Guid.NewGuid(),
                TenantId: Guid.NewGuid(),
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid());

        public static AssignPermissionCommand CreateAssignPermissionCommand()
            => new(
                UserId: Guid.NewGuid(),
                TenantId: Guid.NewGuid(),
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid(),
                Permission: ShowtimePermisions.ScheduleShowtime);

        public static RemovePermissionCommand CreateRemovePermissionCommand()
            => new(
                UserId: Guid.NewGuid(),
                TenantId: Guid.NewGuid(),
                PartitionId: Guid.NewGuid(),
                CinemaId: Guid.NewGuid(),
                Permission: ShowtimePermisions.GetShowtimes);
    }
}