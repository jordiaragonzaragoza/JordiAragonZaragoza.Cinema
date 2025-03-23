namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Movie.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Movie.Responses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    using IMapper = AutoMapper.IMapper;

    // TODO: It belongs to the cinema management bounded context.
    public sealed class GetMovies : Endpoint<GetMoviesRequest, PaginatedCollectionResponse<MovieResponse>>
    {
        public const string Route = "movies";

        private readonly IQueryBus queryBus;
        private readonly IMapper mapper;

        public GetMovies(IQueryBus queryBus, IMapper mapper)
        {
            this.queryBus = Guard.Against.Null(queryBus, nameof(queryBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetMovies.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Movies. Temporal: It belongs to the cinema management bounded context.";
                summary.Description = "Gets a list of all Movies";
            });
        }

        public override async Task HandleAsync(GetMoviesRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.queryBus.SendAsync(this.mapper.Map<GetMoviesQuery>(req), ct);

            var resultResponse = this.mapper.Map<Result<PaginatedCollectionResponse<MovieResponse>>>(resultOutputDto);
            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}