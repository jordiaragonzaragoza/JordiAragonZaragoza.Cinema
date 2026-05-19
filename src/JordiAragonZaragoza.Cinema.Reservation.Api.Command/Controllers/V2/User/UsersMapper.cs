namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V2.User
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;

    public static class UsersMapper
    {
        public static GrantUserCommand ToCommand(this GrantUserBodyRequest request, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new GrantUserCommand(
                userId,
                request.TenantId,
                request.PartitionId,
                request.CinemaId,
                request.Roles,
                request.Permissions);
        }

        public static RevokeUserCommand ToCommand(this RevokeUserBodyRequest request, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new RevokeUserCommand(
                userId,
                request.TenantId,
                request.PartitionId,
                request.CinemaId);
        }

        public static AssignRoleCommand ToCommand(this AssignRoleBodyRequest request, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new AssignRoleCommand(
                userId,
                request.TenantId,
                request.PartitionId,
                request.CinemaId,
                request.Role);
        }

        public static RemoveRoleCommand ToCommand(this RemoveRoleBodyRequest request, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new RemoveRoleCommand(
                userId,
                request.TenantId,
                request.PartitionId,
                request.CinemaId,
                request.Role);
        }

        public static AssignPermissionCommand ToCommand(this AssignPermissionBodyRequest request, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new AssignPermissionCommand(
                userId,
                request.TenantId,
                request.PartitionId,
                request.CinemaId,
                request.Permission);
        }

        public static RemovePermissionCommand ToCommand(this RemovePermissionBodyRequest request, Guid userId)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new RemovePermissionCommand(
                userId,
                request.TenantId,
                request.PartitionId,
                request.CinemaId,
                request.Permission);
        }
    }
}