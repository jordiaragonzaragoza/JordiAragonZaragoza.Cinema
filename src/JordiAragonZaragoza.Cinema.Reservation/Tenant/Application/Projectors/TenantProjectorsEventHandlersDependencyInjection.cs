namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Projectors
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Projectors.Tenant;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain.Events;

    public static class TenantProjectorsEventHandlersDependencyInjection
    {
        public static IServiceCollection AddTenantProjectors(this IServiceCollection services)
        {
            // Tenant projection.
            services.AddProjectorEventHandler<TenantCreatedEvent, TenantAddedEventProjector>();
            services.AddProjectorEventHandler<TenantRemovedEvent, TenantRemovedEventProjector>();

            return services;
        }
    }
}