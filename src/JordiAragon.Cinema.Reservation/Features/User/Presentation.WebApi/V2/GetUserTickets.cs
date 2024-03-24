/*namespace JordiAragon.Cinema.Reservation.User.Presentation.WebApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.User.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.User.Responses;
    using JordiAragon.SharedKernel.Presentation.WebApi.Contracts;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;

    using IMapper = AutoMapper.IMapper;

    public class GetUserTickets : Endpoint<UserTicketsRequest, PaginatedCollectionResponse<UserTicketResponse>>
    {
        public const string Route = "users/{userId}/showtimes/{showtimeId}/tickets";

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
                summary.Summary = "Gets a list of all Users";
                summary.Description = "Gets a list of all Users";
            });
        }

        public override async Task HandleAsync(UserTicketsRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.internalBus.Send(new GetUserTicketsQuery(), ct);

            var resultResponse = this.mapper.Map<Result<PaginatedCollectionResponse<UserTicketResponse>>>(resultOutputDto);
            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}*/