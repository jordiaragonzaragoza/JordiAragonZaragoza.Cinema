namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public sealed class GetShowtime : Endpoint<GetShowtimeRequest, ShowtimeResponse>
    {
        public const string Route = "showtimes/{showtimeId}";

        private readonly ISender internalBus;
        private readonly IMapper mapper;

        public GetShowtime(ISender internalBus, IMapper mapper)
        {
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
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
            var resultOutputDto = await this.internalBus.Send(this.mapper.Map<GetShowtimeQuery>(req), ct);

            var resultResponse = this.mapper.Map<Result<ShowtimeResponse>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}