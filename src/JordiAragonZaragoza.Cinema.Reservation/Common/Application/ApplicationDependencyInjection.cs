namespace JordiAragonZaragoza.Cinema.Reservation.Common.Application
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Projectors;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.QueryHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Projectors;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.QueryHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(AssemblyReference.Assembly, ServiceLifetime.Singleton);

            return services;
        }

        public static IServiceCollection AddApplicationCommandHandlers(this IServiceCollection services)
        {
            services.AddAuditoriumCommandHandlers();
            services.AddMovieCommandHandlers();
            services.AddShowtimeCommandHandlers();
            services.AddUserCommandHandlers();

            return services;
        }

        public static IServiceCollection AddApplicationQueryHandlers(this IServiceCollection services)
        {
            services.AddAuditoriumQueryHandlers();
            services.AddMovieQueryHandlers();
            services.AddShowtimeQueryHandlers();
            services.AddUserQueryHandlers();

            return services;
        }

        public static IServiceCollection AddApplicationProjectorsEventHandlers(this IServiceCollection services)
        {
            services.AddShowtimeProjectors();
            services.AddAuditoriumProjectors();
            services.AddMovieProjectors();
            services.AddUserProjectors();

            return services;
        }
    }
}