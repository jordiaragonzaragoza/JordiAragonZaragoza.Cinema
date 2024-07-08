namespace JordiAragon.Cinema.Reservation.User.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.User.Requests;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    using IMapper = AutoMapper.IMapper;

    public sealed class GetUserTickets : Endpoint<UserTicketsRequest, PaginatedCollectionResponse<TicketResponse>>
    {
        public const string Route = "users/{userId}/tickets";

        private readonly IQueryBus queryBus;
        private readonly IMapper mapper;

        public GetUserTickets(IQueryBus queryBus, IMapper mapper)
        {
            this.queryBus = Guard.Against.Null(queryBus, nameof(queryBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetUserTickets.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a list of user tickets";
                summary.Description = "Gets a list of user tickets";
            });
        }

        public override async Task HandleAsync(UserTicketsRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.queryBus.SendAsync(this.mapper.Map<GetUserTicketsQuery>(req), ct);

            var resultResponse = this.mapper.Map<Result<PaginatedCollectionResponse<TicketResponse>>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}