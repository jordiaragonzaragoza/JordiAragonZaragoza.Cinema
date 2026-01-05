namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors
{
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.AvailableSeat;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Reservation;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Showtime;
    using Microsoft.Extensions.DependencyInjection;

    public static class ShowtimeProjectorsEventHandlersDependencyInjection
    {
        public static IServiceCollection AddShowtimeProjectors(this IServiceCollection services)
        {
            // Showtime projection.
            services.AddShowtimeProjectorsEventHandlers();

            // Available seat projection.
            services.AddAvailableSeatProjectorsEventHandlers();

            // Reservation projection.
            services.AddReservationProjectorsEventHandlers();

            return services;
        }
    }
}