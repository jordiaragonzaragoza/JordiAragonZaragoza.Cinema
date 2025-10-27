namespace JordiAragonZaragoza.Cinema.Reservation.Common.Application
{
    using FluentValidation;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(AssemblyReference.Assembly, ServiceLifetime.Singleton);

            return services;
        }
    }
}