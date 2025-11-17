namespace JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;

    public static class UserRepositories
    {
        public static IServiceCollection AddUserProjectionsRepositories(this IServiceCollection services)
        {
            // UserReadModel projection.
            services.AddScoped<IRepository<UserReadModel, Guid>, ReservationReadModelRepository<UserReadModel>>();

            return services;
        }
    }
}