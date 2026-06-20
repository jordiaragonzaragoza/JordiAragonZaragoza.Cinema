namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers
{
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorizations;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserReservation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserReservations;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUsers;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using Microsoft.Extensions.DependencyInjection;

    public static class UserQueryHandlersDependencyInjection
    {
        public static IServiceCollection AddUserQueryHandlers(this IServiceCollection services)
        {
            services.AddQueryHandler<GetUsersQuery, PaginatedCollectionOutputDto<UserReadModel>, GetUsersQueryHandler>();
            services.AddQueryHandler<GetUserAuthorizationsQuery, IReadOnlyCollection<UserAuthorizationReadModel>, GetUserAuthorizationsQueryHandler>();
            services.AddQueryHandler<GetUserReservationQuery, ReservationReadModel, GetUserReservationQueryHandler>();
            services.AddQueryHandler<GetUserReservationsQuery, PaginatedCollectionOutputDto<ReservationReadModel>, GetUserReservationsQueryHandler>();

            return services;
        }
    }
}