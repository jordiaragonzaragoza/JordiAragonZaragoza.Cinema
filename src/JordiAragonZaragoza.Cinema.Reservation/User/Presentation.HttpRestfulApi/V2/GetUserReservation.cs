namespace JordiAragonZaragoza.Cinema.Reservation.User.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    using IMapper = AutoMapper.IMapper;

    public sealed class GetUserReservation : Endpoint<UserReservationRequest, ReservationResponse>
    {
        public const string Route = "users/{userId}/showtimes/{showtimeId}/reservations/{reservationId}";

        private readonly IQueryBus queryBus;
        private readonly IMapper mapper;

        public GetUserReservation(IQueryBus queryBus, IMapper mapper)
        {
            this.queryBus = Guard.Against.Null(queryBus, nameof(queryBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetUserReservation.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a user reservation for existing showtime";
                summary.Description = "Gets a user reservation for existing showtime";
            });
        }

        public override async Task HandleAsync(UserReservationRequest req, CancellationToken ct)
        {
            var query = this.mapper.Map<GetUserReservationQuery>(req);

            var resultOutputDto = await this.queryBus.SendAsync(query, ct);

            var resultResponse = this.mapper.Map<Result<ReservationResponse>>(resultOutputDto);
            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}