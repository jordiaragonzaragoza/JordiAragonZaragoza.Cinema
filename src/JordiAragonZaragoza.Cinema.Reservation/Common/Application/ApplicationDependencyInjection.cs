namespace JordiAragonZaragoza.Cinema.Reservation.Common.Application
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers;
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

            return services;
        }
    }
}