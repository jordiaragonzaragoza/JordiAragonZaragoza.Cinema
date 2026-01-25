namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.CancelShowtime;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.ScheduleShowtime;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.EndShowtime;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.ExpireReservedSeats;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.PurchaseReservation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.ReserveSeats;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;

    public static class ShowtimeCommandHandlersDependencyInjection
    {
        public static IServiceCollection AddShowtimeCommandHandlers(this IServiceCollection services)
        {
            services.AddCommandHandler<ScheduleShowtimeCommand, ScheduleShowtimeCommandHandler>();
            services.AddCommandHandler<CancelShowtimeCommand, CancelShowtimeCommandHandler>();
            services.AddCommandHandler<EndShowtimeCommand, EndShowtimeCommandHandler>();
            services.AddCommandHandler<ExpireReservedSeatsCommand, ExpireReservedSeatsCommandHandler>();
            services.AddCommandHandler<PurchaseReservationCommand, PurchaseReservationCommandHandler>();
            services.AddCommandHandler<ReserveSeatsCommand, ReservationOutputDto, ReserveSeatsCommandHandler>();

            return services;
        }
    }
}