namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers
{
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetAvailableSeats;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtime;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimeReservations;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimes;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using Microsoft.Extensions.DependencyInjection;

    public static class ShowtimeQueryHandlersDependencyInjection
    {
        public static IServiceCollection AddShowtimeQueryHandlers(this IServiceCollection services)
        {
            services.AddQueryHandler<GetAvailableSeatsQuery, IEnumerable<AvailableSeatReadModel>, GetAvailableSeatsQueryHandler>();
            services.AddQueryHandler<GetShowtimeQuery, ShowtimeReadModel, GetShowtimeQueryHandler>();
            services.AddQueryHandler<GetShowtimeReservationsQuery, PaginatedCollectionOutputDto<ReservationReadModel>, GetShowtimeReservationsQueryHandler>();
            services.AddQueryHandler<GetShowtimesQuery, PaginatedCollectionOutputDto<ShowtimeReadModel>, GetShowtimesQueryHandler>();

            return services;
        }
    }
}