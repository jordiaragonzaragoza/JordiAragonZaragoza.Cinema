namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors.User;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors.UserAuthorization;

    public static class UserProjectorsEventHandlersDependencyInjection
    {
        public static IServiceCollection AddUserProjectors(this IServiceCollection services)
        {
            // User projection.
            services.AddUserProjectorsEventHandlers();

            // User authorization projection.
            services.AddUserAuthorizationProjectorsEventHandlers();

            return services;
        }
    }
}