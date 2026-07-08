namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public static class PartitionRepositories
    {
        public static IServiceCollection AddPartitionProjectionsRepositories(this IServiceCollection services)
        {
            // PartitionReadModel projection.
            services.AddScoped<IRepository<PartitionReadModel, Guid>, ReservationReadModelRepository<PartitionReadModel>>();
            services.AddScoped<IPaginatedSpecificationReadRepository<PartitionReadModel>, ReservationReadModelRepository<PartitionReadModel>>();

            return services;
        }
    }
}