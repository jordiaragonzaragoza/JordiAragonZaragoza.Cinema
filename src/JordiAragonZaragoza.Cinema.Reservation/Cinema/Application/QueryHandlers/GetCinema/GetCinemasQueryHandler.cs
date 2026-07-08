namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.QueryHandlers.GetCinemas
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    // TODO: Temporal. Move. This query is part of other bounded context(Catalog)
    public sealed class GetCinemasQueryHandler : IQueryHandler<GetCinemasQuery, PaginatedCollectionOutputDto<CinemaReadModel>>
    {
        private readonly IPaginatedSpecificationReadRepository<CinemaReadModel> auditoriumReadModelRepository;

        public GetCinemasQueryHandler(IPaginatedSpecificationReadRepository<CinemaReadModel> auditoriumReadModelRepository)
        {
            this.auditoriumReadModelRepository = auditoriumReadModelRepository ?? throw new ArgumentNullException(nameof(auditoriumReadModelRepository));
        }

        public async Task<Result<PaginatedCollectionOutputDto<CinemaReadModel>>> Handle(GetCinemasQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetCinemasSpec(request);
            var result = await this.auditoriumReadModelRepository.PaginatedListAsync(specification, cancellationToken);
            if (!result.Items.Any())
            {
                return Result.NotFound("Cinema/s not found.");
            }

            return Result.Success(result);
        }
    }
}