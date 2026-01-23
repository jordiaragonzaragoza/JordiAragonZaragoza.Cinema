namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.AddMovie;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.RemoveMovie;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.ScheduleShowtimeInMovie;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.CancelShowtimeInMovie;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.EndShowtimeInMovie;

    public static class MovieCommandHandlersDependencyInjection
    {
        public static IServiceCollection AddMovieCommandHandlers(this IServiceCollection services)
        {
            services.AddCommandHandler<AddMovieCommand, AddMovieCommandHandler>();
            services.AddCommandHandler<RemoveMovieCommand, RemoveMovieCommandHandler>();
            services.AddCommandHandler<ScheduleShowtimeInMovieCommand, ScheduleShowtimeInMovieCommandHandler>();
            services.AddCommandHandler<CancelShowtimeInMovieCommand, CancelShowtimeInMovieCommandHandler>();
            services.AddCommandHandler<EndShowtimeInMovieCommand, EndShowtimeInMovieCommandHandler>();

            return services;
        }
    }
}