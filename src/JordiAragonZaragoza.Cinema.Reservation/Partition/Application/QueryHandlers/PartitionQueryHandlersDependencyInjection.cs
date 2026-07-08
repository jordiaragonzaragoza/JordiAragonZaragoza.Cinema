namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.QueryHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.QueryHandlers.GetPartitions;

    public static class PartitionQueryHandlersDependencyInjection
    {
        public static IServiceCollection AddPartitionQueryHandlers(this IServiceCollection services)
        {
            services.AddQueryHandler<GetPartitionsQuery, PaginatedCollectionOutputDto<PartitionReadModel>, GetPartitionsQueryHandler>();

            return services;
        }
    }
}