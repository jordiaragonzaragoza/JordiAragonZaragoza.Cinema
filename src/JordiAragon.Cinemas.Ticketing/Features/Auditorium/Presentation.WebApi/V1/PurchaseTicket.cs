namespace JordiAragon.Cinemas.Ticketing.Auditorium.Presentation.WebApi.V1
{
    using System.Threading;
    using System.Threading.Tasks;
    using FastEndpoints;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;

    public class PurchaseTicket : Endpoint<PurchaseTicketRequest>
    {
        private readonly ISender sender;

        public PurchaseTicket(ISender sender)
        {
            this.sender = sender;
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
            var resultResponse = await this.sender.Send(new PurchaseSeatsCommand(req.ShowtimeId, req.TicketId), ct);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}