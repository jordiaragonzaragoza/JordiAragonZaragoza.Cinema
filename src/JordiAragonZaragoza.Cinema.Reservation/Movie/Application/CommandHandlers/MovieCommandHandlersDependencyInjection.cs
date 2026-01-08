namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.AddMovie;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.RemoveMovie;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.AddActiveShowtime;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.RemoveActiveShowtime;

    public static class MovieCommandHandlersDependencyInjection
    {
        public static IServiceCollection AddMovieCommandHandlers(this IServiceCollection services)
        {
            services.AddCommandHandler<AddMovieCommand, AddMovieCommandHandler>();
            services.AddCommandHandler<RemoveMovieCommand, RemoveMovieCommandHandler>();
            services.AddCommandHandler<AddActiveShowtimeCommand, AddActiveShowtimeCommandHandler>();
            services.AddCommandHandler<RemoveActiveShowtimeCommand, RemoveActiveShowtimeCommandHandler>();

            return services;
        }
    }
}