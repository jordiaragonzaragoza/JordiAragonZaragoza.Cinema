namespace JordiAragon.Cinema.Reservation.User.Presentation.WebApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.User.Requests;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;

    using IMapper = AutoMapper.IMapper;

    public class GetUserTicket : Endpoint<UserTicketRequest, TicketResponse>
    {
        public const string Route = "users/{userId}/showtimes/{showtimeId}/tickets/{ticketId}";

        private readonly ISender internalBus;
        private readonly IMapper mapper;

        public GetUserTicket(ISender internalBus, IMapper mapper)
        {
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Get(GetUserTicket.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Gets a user ticket for existing showtime";
                summary.Description = "Gets a user ticket for existing showtime";
            });
        }

        public override async Task HandleAsync(UserTicketRequest req, CancellationToken ct)
        {
            var query = this.mapper.Map<GetUserTicketQuery>(req);

            var resultOutputDto = await this.internalBus.Send(query, ct);

            var resultResponse = this.mapper.Map<Result<TicketResponse>>(resultOutputDto);
            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}