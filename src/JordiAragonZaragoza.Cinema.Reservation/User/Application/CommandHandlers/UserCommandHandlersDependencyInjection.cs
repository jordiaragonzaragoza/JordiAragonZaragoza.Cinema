namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RemoveUser;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.CreateUser;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.GrantUser;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RevokeUser;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.AssignRole;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RemoveRole;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.AssignPermission;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RemovePermission;

    public static class UserCommandHandlersDependencyInjection
    {
        public static IServiceCollection AddUserCommandHandlers(this IServiceCollection services)
        {
            services.AddCommandHandler<CreateUserCommand, CreateUserCommandHandler>();
            services.AddCommandHandler<RemoveUserCommand, RemoveUserCommandHandler>();
            services.AddCommandHandler<GrantUserCommand, GrantUserCommandHandler>();
            services.AddCommandHandler<RevokeUserCommand, RevokeUserCommandHandler>();
            services.AddCommandHandler<AssignRoleCommand, AssignRoleCommandHandler>();
            services.AddCommandHandler<RemoveRoleCommand, RemoveRoleCommandHandler>();
            services.AddCommandHandler<AssignPermissionCommand, AssignPermissionCommandHandler>();
            services.AddCommandHandler<RemovePermissionCommand, RemovePermissionCommandHandler>();

            return services;
        }
    }
}