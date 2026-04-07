namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Projectors
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Projectors.Cinema;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain.Events;

    public static class CinemaProjectorsEventHandlersDependencyInjection
    {
        public static IServiceCollection AddCinemaProjectors(this IServiceCollection services)
        {
            // Cinema projection.
            services.AddProjectorEventHandler<CinemaCreatedEvent, CinemaAddedEventProjector>();
            services.AddProjectorEventHandler<CinemaRemovedEvent, CinemaRemovedEventProjector>();

            return services;
        }
    }
}