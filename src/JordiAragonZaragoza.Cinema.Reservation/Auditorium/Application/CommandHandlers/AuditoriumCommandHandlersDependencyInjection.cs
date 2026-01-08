namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.CreateAuditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.RemoveAuditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.AddActiveShowtime;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.RemoveActiveShowtime;

    public static class AuditoriumCommandHandlersDependencyInjection
    {
        public static IServiceCollection AddAuditoriumCommandHandlers(this IServiceCollection services)
        {
            services.AddCommandHandler<CreateAuditoriumCommand, CreateAuditoriumCommandHandler>();
            services.AddCommandHandler<RemoveAuditoriumCommand, RemoveAuditoriumCommandHandler>();
            services.AddCommandHandler<AddActiveShowtimeCommand, AddActiveShowtimeCommandHandler>();
            services.AddCommandHandler<RemoveActiveShowtimeCommand, RemoveActiveShowtimeCommandHandler>();

            return services;
        }
    }
}