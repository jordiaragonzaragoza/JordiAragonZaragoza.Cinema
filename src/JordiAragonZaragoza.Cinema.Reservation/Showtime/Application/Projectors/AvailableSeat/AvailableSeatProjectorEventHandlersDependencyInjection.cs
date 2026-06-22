namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.AvailableSeat
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;

    public static class AvailableSeatProjectorEventHandlersDependencyInjection
    {
        public static IServiceCollection AddAvailableSeatProjectorsEventHandlers(this IServiceCollection services)
        {
            // Available seat projection.
            services.AddProjectorEventHandler<ExpiredReservedSeatsEvent, ExpiredReservedSeatsEventProjector>();
            services.AddProjectorEventHandler<ReservationCanceledEvent, ReservationCanceledEventProjector>();
            services.AddProjectorEventHandler<ReservedSeatsEvent, ReservedSeatsEventProjector>();
            services.AddProjectorEventHandler<ShowtimeCanceledEvent, ShowtimeCanceledEventProjector>();
            services.AddProjectorEventHandler<ShowtimeEndedEvent, ShowtimeEndedEventProjector>();
            services.AddProjectorEventHandler<ShowtimeScheduledEvent, ShowtimeScheduledEventProjector>();

            return services;
        }
    }
}