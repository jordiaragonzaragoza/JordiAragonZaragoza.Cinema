namespace JordiAragon.Cinema.Reservation.User.Presentation.WebApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.User.Requests;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Presentation.WebApi.Contracts;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;

    using IMapper = AutoMapper.IMapper;

    public class GetUserTickets : Endpoint<UserTicketsRequest, PaginatedCollectionResponse<TicketResponse>>
    {
        public const string Route = "users/{userId}/tickets";

        private readonly ISender internalBus;
        private readonly IMapper mapper;

        public GetUserTickets(ISender internalBus, IMapper mapper)
        {
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
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
            var resultOutputDto = await this.internalBus.Send(this.mapper.Map<GetUserTicketsQuery>(req), ct);

            var resultResponse = this.mapper.Map<Result<PaginatedCollectionResponse<TicketResponse>>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}