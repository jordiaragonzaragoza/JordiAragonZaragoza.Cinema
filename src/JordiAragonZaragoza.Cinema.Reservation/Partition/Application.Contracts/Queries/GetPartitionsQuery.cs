namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.Queries
{
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;

    public sealed record class GetPartitionsQuery(
        int PageNumber,
        int PageSize)
            : IPaginatedQuery, IQuery<PaginatedCollectionOutputDto<PartitionReadModel>>;
}