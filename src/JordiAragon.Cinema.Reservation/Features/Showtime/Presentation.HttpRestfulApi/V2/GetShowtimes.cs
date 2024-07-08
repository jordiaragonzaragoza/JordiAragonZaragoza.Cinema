namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    using IMapper = AutoMapper.IMapper;

    public sealed class GetShowtimes : Endpoint<GetShowtimesRequest, PaginatedCollectionResponse<ShowtimeResponse>>
    {
        public const string Route = "showtimes";

        private readonly IQueryBus queryBus;
        private readonly IMapper mapper;

        public GetShowtimes(IQueryBus queryBus, IMapper mapper)
        {
            this.queryBus = Guard.Against.Null(queryBus, nameof(queryBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetShowtimes.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of all Showtimes";
                summary.Description = "Gets a list of all Showtimes";
            });
        }

        public async override Task HandleAsync(GetShowtimesRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.queryBus.SendAsync(this.mapper.Map<GetShowtimesQuery>(req), ct);

            var resultResponse = this.mapper.Map<Result<PaginatedCollectionResponse<ShowtimeResponse>>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}