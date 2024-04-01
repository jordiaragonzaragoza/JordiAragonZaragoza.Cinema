namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using MediatR;

    public sealed class PurchaseTicket : Endpoint<PurchaseTicketRequest>
    {
        public const string Route = "showtimes/{showtimeId}/tickets/{ticketId}/purchase";

        private readonly ISender internalBus;

        public PurchaseTicket(ISender internalBus)
        {
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Patch(PurchaseTicket.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Purchase a reservation for an existing Showtime. Temporal: This endpoint will not be exposed on finance integration. ";
                summary.Description = "Purchase a reservation for an existing Showtime";
            });
        }

        public async override Task HandleAsync(PurchaseTicketRequest req, CancellationToken ct)
        {
            var result = await this.internalBus.Send(new PurchaseTicketCommand(req.ShowtimeId, req.TicketId), ct);

            await this.SendResponseAsync(result, ct);
        }
    }
}