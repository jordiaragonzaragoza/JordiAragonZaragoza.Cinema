namespace JordiAragon.Cinemas.Ticketing.Showtime.Presentation.WebApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using FastEndpoints;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;

    public class PurchaseTicket : Endpoint<PurchaseTicketRequest>
    {
        public const string Route = "showtimes/{showtimeId}/tickets/{ticketId}/purchase";

        private readonly ISender sender;

        public PurchaseTicket(ISender sender)
        {
            this.sender = sender;
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Patch(PurchaseTicket.Route);
            this.Version(2);
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