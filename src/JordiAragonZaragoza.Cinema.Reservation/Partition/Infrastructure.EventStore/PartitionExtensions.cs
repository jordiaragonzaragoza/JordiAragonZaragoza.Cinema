namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Infrastructure.EventStore
{
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Business;

    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class PartitionExtensions
    {
        public static IServiceCollection AddPartitionBusinessRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Partition, PartitionId>, ReservationRepository<Partition, PartitionId>>();
            services.AddScoped<IReadRepository<Partition, PartitionId>, ReservationRepository<Partition, PartitionId>>();

            return services;
        }
    }
}