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
            => new(Guid.CreateVersion7());

        public static GrantUserCommand CreateGrantUserCommand()
            => new(
                UserId: Guid.CreateVersion7(),
                TenantId: Guid.CreateVersion7(),
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7(),
                Roles: DefaultRoles,
                Permissions: Permissions);

        public static AssignRoleCommand CreateAssignRoleCommand()
            => new(
                UserId: Guid.CreateVersion7(),
                TenantId: Guid.CreateVersion7(),
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7(),
                Role: Constants.Role.Viewer);

        public static RemoveRoleCommand CreateRemoveRoleCommand()
            => new(
                UserId: Guid.CreateVersion7(),
                TenantId: Guid.CreateVersion7(),
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7(),
                Role: Constants.Role.Viewer);

        public static RemoveUserCommand CreateRemoveUserCommand()
            => new(Guid.CreateVersion7());

        public static RevokeUserCommand CreateRevokeUserCommand()
            => new(
                UserId: Guid.CreateVersion7(),
                TenantId: Guid.CreateVersion7(),
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7());

        public static AssignPermissionCommand CreateAssignPermissionCommand()
            => new(
                UserId: Guid.CreateVersion7(),
                TenantId: Guid.CreateVersion7(),
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7(),
                Permission: ShowtimePermisions.ScheduleShowtime);

        public static RemovePermissionCommand CreateRemovePermissionCommand()
            => new(
                UserId: Guid.CreateVersion7(),
                TenantId: Guid.CreateVersion7(),
                PartitionId: Guid.CreateVersion7(),
                CinemaId: Guid.CreateVersion7(),
                Permission: ShowtimePermisions.GetShowtimes);
    }
}