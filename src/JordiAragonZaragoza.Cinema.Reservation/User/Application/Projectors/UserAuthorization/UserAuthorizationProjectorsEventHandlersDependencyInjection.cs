namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors.UserAuthorization
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events;

    public static class UserAuthorizationProjectorsEventHandlersDependencyInjection
    {
        public static IServiceCollection AddUserAuthorizationProjectorsEventHandlers(this IServiceCollection services)
        {
            // User projection.
            services.AddProjectorEventHandler<PermissionAssignedToScopeEvent, PermissionAssignedToScopeEventProjector>();
            services.AddProjectorEventHandler<PermissionRevokedFromScopeEvent, PermissionRevokedFromScopeEventProjector>();
            services.AddProjectorEventHandler<RoleAssignedToScopeEvent, RoleAssignedToScopeEventProjector>();
            services.AddProjectorEventHandler<RoleRevokedFromScopeEvent, RoleRevokedFromScopeEventProjector>();
            services.AddProjectorEventHandler<UserGrantedEvent, UserGrantedEventProjector>();
            services.AddProjectorEventHandler<UserRemovedEvent, UserRemovedEventProjector>();
            services.AddProjectorEventHandler<UserRevokedEvent, UserRevokedEventProjector>();

            return services;
        }
    }
}