namespace JordiAragonZaragoza.Cinema.Reservation.Common.Application
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
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
    }
}