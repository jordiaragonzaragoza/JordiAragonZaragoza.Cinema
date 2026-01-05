namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors.User;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events;

    public static class UserProjectorsEventHandlersDependencyInjection
    {
        public static IServiceCollection AddUserProjectors(this IServiceCollection services)
        {
            // User projection.
            services.AddProjectorEventHandler<UserCreatedEvent, UserAddedEventProjector>();
            services.AddProjectorEventHandler<UserRemovedEvent, UserRemovedEventProjector>();

            return services;
        }
    }
}