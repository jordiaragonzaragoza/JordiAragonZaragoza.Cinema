namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    using IMapper = AutoMapper.IMapper;

    public sealed class GetShowtime : Endpoint<GetShowtimeRequest, ShowtimeResponse>
    {
        public const string Route = "showtimes/{showtimeId}";

        private readonly IQueryBus queryBus;
        private readonly IMapper mapper;

        public GetShowtime(IQueryBus queryBus, IMapper mapper)
        {
            this.queryBus = Guard.Against.Null(queryBus, nameof(queryBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetShowtime.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Get an existing showtime";
                summary.Description = "Get an existing showtime";
            });
        }

        public async override Task HandleAsync(GetShowtimeRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.queryBus.SendAsync(this.mapper.Map<GetShowtimeQuery>(req), ct);

            var resultResponse = this.mapper.Map<Result<ShowtimeResponse>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}