namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;

    public static class ShowtimeRepositories
    {
        public static IServiceCollection AddShowtimeProjectionsRepositories(this IServiceCollection services)
        {
            // ShowtimeReadModel projection.
            services.AddScoped<IRepository<ShowtimeReadModel, Guid>, ReservationReadModelRepository<ShowtimeReadModel>>();
            services.AddScoped<IReadRepository<ShowtimeReadModel, Guid>, ReservationReadModelRepository<ShowtimeReadModel>>();

            // AvailableSeatReadModel projection.
            services.AddScoped<IRangeableRepository<AvailableSeatReadModel, Guid>, ReservationReadModelRepository<AvailableSeatReadModel>>();
            services.AddScoped<ISpecificationReadRepository<AvailableSeatReadModel, Guid>, ReservationReadModelRepository<AvailableSeatReadModel>>();

            // ReservationReadModel projection.
            services.AddScoped<IRepository<ReservationReadModel, Guid>, ReservationReadModelRepository<ReservationReadModel>>();
            services.AddScoped<ISpecificationReadRepository<ReservationReadModel, Guid>, ReservationReadModelRepository<ReservationReadModel>>();
            services.AddScoped<IRangeableRepository<ReservationReadModel, Guid>, ReservationReadModelRepository<ReservationReadModel>>();

            return services;
        }
    }
}