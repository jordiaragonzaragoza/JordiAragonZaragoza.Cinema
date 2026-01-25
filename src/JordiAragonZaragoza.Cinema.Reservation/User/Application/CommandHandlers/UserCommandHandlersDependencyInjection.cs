namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RemoveUser;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.CreateUser;

    public static class UserCommandHandlersDependencyInjection
    {
        public static IServiceCollection AddUserCommandHandlers(this IServiceCollection services)
        {
            services.AddCommandHandler<CreateUserCommand, CreateUserCommandHandler>();
            services.AddCommandHandler<RemoveUserCommand, RemoveUserCommandHandler>();

            return services;
        }
    }
}