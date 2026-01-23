namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Projectors
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Projectors.Auditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Events;

    public static class AuditoriumProjectorsEventHandlersDependencyInjection
    {
        public static IServiceCollection AddAuditoriumProjectors(this IServiceCollection services)
        {
            // Auditorium projection.
            services.AddProjectorEventHandler<AuditoriumCreatedEvent, AuditoriumCreatedEventProjector>();
            services.AddProjectorEventHandler<AuditoriumRemovedEvent, AuditoriumRemovedEventProjector>();

            return services;
        }
    }
}