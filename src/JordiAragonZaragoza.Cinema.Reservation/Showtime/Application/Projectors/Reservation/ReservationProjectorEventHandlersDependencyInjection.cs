namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Reservation
{
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using Microsoft.Extensions.DependencyInjection;

    public static class ReservationProjectorEventHandlersDependencyInjection
    {
        public static IServiceCollection AddReservationProjectorsEventHandlers(this IServiceCollection services)
        {
            // Reservation projection.
            services.AddProjectorEventHandler<ExpiredReservedSeatsEvent, ExpiredReservedSeatsEventProjector>();
            services.AddProjectorEventHandler<ReservationCanceledEvent, ReservationCanceledEventProjector>();
            services.AddProjectorEventHandler<PurchasedReservationEvent, PurchasedReservationEventProjector>();
            services.AddProjectorEventHandler<ReservedSeatsEvent, ReservedSeatsEventProjector>();
            services.AddProjectorEventHandler<ShowtimeCanceledEvent, ShowtimeCanceledEventProjector>();

            return services;
        }
    }
}