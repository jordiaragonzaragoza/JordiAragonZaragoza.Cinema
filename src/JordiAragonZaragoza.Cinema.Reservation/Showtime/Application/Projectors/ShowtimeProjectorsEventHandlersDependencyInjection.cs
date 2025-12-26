namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;

    public static class ShowtimeProjectorsEventHandlersDependencyInjection
    {
        public static IServiceCollection AddShowtimeProjectorsEventHandlers(this IServiceCollection services)
        {
            // Showtime projection.
            services.AddProjectorEventHandler<ShowtimeScheduledEvent, ShowtimeScheduledEventProjector>();
            services.AddProjectorEventHandler<ShowtimeCanceledEvent, ShowtimeCanceledEventProjector>();

            return services;
        }
    }
}