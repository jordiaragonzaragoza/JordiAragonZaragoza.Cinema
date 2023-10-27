namespace JordiAragon.Cinemas.Ticketing.Auditorium.Presentation.WebApi.V1
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Ticket.Requests;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Ticket.Responses;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class ReserveSeats : Endpoint<ReserveSeatsRequest, TicketResponse>
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public ReserveSeats(ISender sender, IMapper mapper)
        {
            this.sender = Guard.Against.Null(sender, nameof(sender));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Post("auditoriums/{auditoriumId}/showtimes/{showtimeId}/tickets");
            this.Version(1, 2);
            this.Summary(summary =>
            {
                summary.Summary = "Reserve Seats for an existing Showtime";
                summary.Description = "Reserve Seats for an existing Showtime";
            });
        }

        public async override Task HandleAsync(ReserveSeatsRequest req, CancellationToken ct)
        {
            var command = this.mapper.Map<ReserveSeatsCommand>(req);

            var resultOutputDto = await this.sender.Send(command, ct);

            var resultResponse = this.mapper.Map<Result<TicketResponse>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}