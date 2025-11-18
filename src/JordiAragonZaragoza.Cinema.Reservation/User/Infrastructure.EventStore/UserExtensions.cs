namespace JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EventStore
{
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Business;

    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class UserExtensions
    {
        public static IServiceCollection AddUserBusinessRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<User, UserId>, ReservationRepository<User, UserId>>();
            services.AddScoped<IReadRepository<User, UserId>, ReservationRepository<User, UserId>>();

            return services;
        }
    }
}