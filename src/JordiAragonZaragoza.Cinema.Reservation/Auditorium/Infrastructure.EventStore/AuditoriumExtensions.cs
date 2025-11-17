namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EventStore
{
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Business;

    public static class AuditoriumExtensions
    {
        public static IServiceCollection AddAuditorium(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Auditorium, AuditoriumId>, ReservationRepository<Auditorium, AuditoriumId>>();
            services.AddScoped<IReadRepository<Auditorium, AuditoriumId>, ReservationRepository<Auditorium, AuditoriumId>>();

            return services;
        }
    }
}