namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers
{
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.CancelShowtimeInAuditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.CreateAuditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.EndShowtimeInAuditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.RemoveAuditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.ScheduleShowtimeInAuditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using Microsoft.Extensions.DependencyInjection;

    public static class AuditoriumCommandHandlersDependencyInjection
    {
        public static IServiceCollection AddAuditoriumCommandHandlers(this IServiceCollection services)
        {
            services.AddCommandHandler<CreateAuditoriumCommand, CreateAuditoriumCommandHandler>();
            services.AddCommandHandler<RemoveAuditoriumCommand, RemoveAuditoriumCommandHandler>();
            services.AddCommandHandler<ScheduleShowtimeInAuditoriumCommand, ScheduleShowtimeInAuditoriumCommandHandler>();
            services.AddCommandHandler<CancelShowtimeInAuditoriumCommand, CancelShowtimeInAuditoriumCommandHandler>();
            services.AddCommandHandler<EndShowtimeInAuditoriumCommand, EndShowtimeInAuditoriumCommandHandler>();

            return services;
        }
    }
}