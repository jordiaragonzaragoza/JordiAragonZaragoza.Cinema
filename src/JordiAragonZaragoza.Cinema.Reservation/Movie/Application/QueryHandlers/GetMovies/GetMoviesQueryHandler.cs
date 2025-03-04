namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.QueryHandlers.GetMovies
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    // TODO: Temporal. Move. This query is part of other bounded context(Catalog)
    public sealed class GetMoviesQueryHandler : IQueryHandler<GetMoviesQuery, PaginatedCollectionOutputDto<MovieReadModel>>
    {
        private readonly IPaginatedSpecificationReadRepository<MovieReadModel> auditoriumReadModelRepository;

        public GetMoviesQueryHandler(IPaginatedSpecificationReadRepository<MovieReadModel> auditoriumReadModelRepository)
        {
            this.auditoriumReadModelRepository = Guard.Against.Null(auditoriumReadModelRepository, nameof(auditoriumReadModelRepository));
        }

        public async Task<Result<PaginatedCollectionOutputDto<MovieReadModel>>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetMoviesSpec(request);
            var result = await this.auditoriumReadModelRepository.PaginatedListAsync(specification, cancellationToken);
            if (!result.Items.Any())
            {
                return Result.NotFound("Movie/s not found.");
            }

            return Result.Success(result);
        }
    }
}