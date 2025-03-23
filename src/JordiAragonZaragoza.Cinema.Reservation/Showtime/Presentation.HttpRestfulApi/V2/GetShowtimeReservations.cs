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
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    using IMapper = AutoMapper.IMapper;

    public sealed class GetShowtimeReservations : Endpoint<GetShowtimeReservationsRequest, PaginatedCollectionResponse<ReservationResponse>>
    {
        public const string Route = "showtimes/{showtimeId}/reservations";

        private readonly IQueryBus queryBus;
        private readonly IMapper mapper;

        public GetShowtimeReservations(IQueryBus queryBus, IMapper mapper)
        {
            this.queryBus = Guard.Against.Null(queryBus, nameof(queryBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetShowtimeReservations.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of reservations for an exiting showtime";
                summary.Description = "Gets a list of reservations for an exiting showtime";
            });
        }

        public async override Task HandleAsync(GetShowtimeReservationsRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.queryBus.SendAsync(this.mapper.Map<GetShowtimeReservationsQuery>(req), ct);

            var resultResponse = this.mapper.Map<Result<PaginatedCollectionResponse<ReservationResponse>>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}