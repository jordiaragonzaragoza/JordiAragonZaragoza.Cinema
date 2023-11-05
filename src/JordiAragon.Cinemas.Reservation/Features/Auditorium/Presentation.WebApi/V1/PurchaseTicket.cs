namespace JordiAragon.Cinemas.Reservation.Auditorium.Presentation.WebApi.V1
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using FastEndpoints;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Ticket.Responses;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;
    using IMapper = AutoMapper.IMapper;

    public class PurchaseTicket : Endpoint<PurchaseTicketRequest, TicketResponse>
    {
        private readonly ISender sender;
        private readonly IMapper mapper;

        public PurchaseTicket(ISender sender, IMapper mapper)
        {
            this.sender = Guard.Against.Null(sender, nameof(sender));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Patch("auditoriums/{auditoriumId}/showtimes/{showtimeId}/tickets/{ticketId}/Purchase");
            this.Version(1, 2);
            this.Summary(summary =>
            {
                summary.Summary = "Purchase a reservation for an existing Showtime";
                summary.Description = "Purchase a reservation for an existing Showtime";
            });
        }

        public async override Task HandleAsync(PurchaseTicketRequest req, CancellationToken ct)
        {
            var resultOutputDto = await this.sender.Send(new PurchaseTicketCommand(req.ShowtimeId, req.TicketId), ct);

            var resultResponse = this.mapper.Map<Result<TicketResponse>>(resultOutputDto);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}