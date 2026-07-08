namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.CommandHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.CommandHandlers.RemoveCinema;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.CommandHandlers.CreateCinema;

    public static class CinemaCommandHandlersDependencyInjection
    {
        public static IServiceCollection AddCinemaCommandHandlers(this IServiceCollection services)
        {
            services.AddCommandHandler<CreateCinemaCommand, CreateCinemaCommandHandler>();
            services.AddCommandHandler<RemoveCinemaCommand, RemoveCinemaCommandHandler>();

            return services;
        }
    }
}