namespace JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EntityFramework
{
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class UserRepositories
    {
        public static IServiceCollection AddUserBusinessModelRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<User, UserId>, ReservationRepository<User, UserId>>();
            services.AddScoped<IReadRepository<User, UserId>, ReservationRepository<User, UserId>>();
            services.AddScoped<IReadListRepository<User, UserId>, ReservationRepository<User, UserId>>();
            services.AddScoped<ISpecificationReadRepository<User, UserId>, ReservationRepository<User, UserId>>();

            return services;
        }
    }
}