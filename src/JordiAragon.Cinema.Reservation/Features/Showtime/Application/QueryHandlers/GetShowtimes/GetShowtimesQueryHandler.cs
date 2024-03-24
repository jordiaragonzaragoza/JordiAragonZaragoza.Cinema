namespace JordiAragon.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimes
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetShowtimesQueryHandler : IQueryHandler<GetShowtimesQuery, PaginatedCollectionOutputDto<ShowtimeReadModel>>
    {
        private readonly IPaginatedSpecificationReadRepository<ShowtimeReadModel> showtimeReadModelRepository;

        public GetShowtimesQueryHandler(IPaginatedSpecificationReadRepository<ShowtimeReadModel> showtimeReadModelRepository)
        {
            this.showtimeReadModelRepository = Guard.Against.Null(showtimeReadModelRepository, nameof(showtimeReadModelRepository));
        }

        public async Task<Result<PaginatedCollectionOutputDto<ShowtimeReadModel>>> Handle(GetShowtimesQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetShowtimesSpec(request);
            var result = await this.showtimeReadModelRepository.PaginatedListAsync(specification, cancellationToken);
            if (!result.Items.Any())
            {
                return Result.NotFound($"{nameof(ShowtimeReadModel)}/s not found.");
            }

            return Result.Success(result);
        }
    }
}