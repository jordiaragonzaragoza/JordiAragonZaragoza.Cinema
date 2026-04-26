namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorization;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUsers;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserReservation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserReservations;

    public static class UserQueryHandlersDependencyInjection
    {
        public static IServiceCollection AddUserQueryHandlers(this IServiceCollection services)
        {
            services.AddQueryHandler<GetUsersQuery, PaginatedCollectionOutputDto<UserReadModel>, GetUsersQueryHandler>();
            services.AddQueryHandler<GetUserAuthorizationQuery, UserAuthorizationReadModel, GetUserAuthorizationQueryHandler>();
            services.AddQueryHandler<GetUserReservationQuery, ReservationReadModel, GetUserReservationQueryHandler>();
            services.AddQueryHandler<GetUserReservationsQuery, PaginatedCollectionOutputDto<ReservationReadModel>, GetUserReservationsQueryHandler>();

            return services;
        }
    }
}