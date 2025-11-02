namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.CreateAuditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.RemoveAuditorium;

    public static class AuditoriumCommandHandlersDependencyInjection
    {
        public static IServiceCollection AddAuditoriumCommandHandlers(this IServiceCollection services)
        {
            services.AddCommandHandler<CreateAuditoriumCommand, CreateAuditoriumCommandHandler>();
            services.AddCommandHandler<RemoveAuditoriumCommand, RemoveAuditoriumCommandHandler>();

            return services;
        }
    }
}