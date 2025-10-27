namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework
{
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class AuditoriumRepositories
    {
        public static IServiceCollection AddAuditoriumBusinessModelRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Auditorium, AuditoriumId>, ReservationRepository<Auditorium, AuditoriumId>>();
            services.AddScoped<IReadRepository<Auditorium, AuditoriumId>, ReservationRepository<Auditorium, AuditoriumId>>();
            services.AddScoped<IReadListRepository<Auditorium, AuditoriumId>, ReservationRepository<Auditorium, AuditoriumId>>();
            services.AddScoped<ISpecificationReadRepository<Auditorium, AuditoriumId>, ReservationRepository<Auditorium, AuditoriumId>>();

            return services;
        }
    }
}