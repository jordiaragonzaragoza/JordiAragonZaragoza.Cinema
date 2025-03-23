namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.QueryHandlers.GetAuditoriums
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    // TODO: Temporal. Move. This query is part of other bounded context(Cinema Management)
    public sealed class GetAuditoriumsQueryHandler : IQueryHandler<GetAuditoriumsQuery, PaginatedCollectionOutputDto<AuditoriumReadModel>>
    {
        private readonly IPaginatedSpecificationReadRepository<AuditoriumReadModel> auditoriumReadModelRepository;

        public GetAuditoriumsQueryHandler(IPaginatedSpecificationReadRepository<AuditoriumReadModel> auditoriumReadModelRepository)
        {
            this.auditoriumReadModelRepository = Guard.Against.Null(auditoriumReadModelRepository, nameof(auditoriumReadModelRepository));
        }

        public async Task<Result<PaginatedCollectionOutputDto<AuditoriumReadModel>>> Handle(GetAuditoriumsQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetAuditoriumsSpec(request);
            var result = await this.auditoriumReadModelRepository.PaginatedListAsync(specification, cancellationToken);
            if (!result.Items.Any())
            {
                return Result.NotFound("Auditorium/s not found.");
            }

            return Result.Success(result);
        }
    }
}