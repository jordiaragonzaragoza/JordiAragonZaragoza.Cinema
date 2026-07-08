namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.QueryHandlers.GetPartitions
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    // TODO: Temporal. Move. This query is part of other bounded context(Catalog)
    public sealed class GetPartitionsQueryHandler : IQueryHandler<GetPartitionsQuery, PaginatedCollectionOutputDto<PartitionReadModel>>
    {
        private readonly IPaginatedSpecificationReadRepository<PartitionReadModel> auditoriumReadModelRepository;

        public GetPartitionsQueryHandler(IPaginatedSpecificationReadRepository<PartitionReadModel> auditoriumReadModelRepository)
        {
            this.auditoriumReadModelRepository = auditoriumReadModelRepository ?? throw new ArgumentNullException(nameof(auditoriumReadModelRepository));
        }

        public async Task<Result<PaginatedCollectionOutputDto<PartitionReadModel>>> Handle(GetPartitionsQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetPartitionsSpec(request);
            var result = await this.auditoriumReadModelRepository.PaginatedListAsync(specification, cancellationToken);
            if (!result.Items.Any())
            {
                return Result.NotFound("Partition/s not found.");
            }

            return Result.Success(result);
        }
    }
}