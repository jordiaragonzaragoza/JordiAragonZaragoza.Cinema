namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    public sealed class PurchaseTicket : Endpoint<PurchaseTicketRequest>
    {
        public const string Route = "showtimes/{showtimeId}/tickets/{ticketId}/purchase";

        private readonly ICommandBus commandBus;

        public PurchaseTicket(ICommandBus queryBus)
        {
            this.commandBus = Guard.Against.Null(queryBus, nameof(queryBus));
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
            Guard.Against.Null(req, nameof(req));

            var result = await this.commandBus.SendAsync(new PurchaseTicketCommand(req.ShowtimeId, req.TicketId), ct);

            await this.SendResponseAsync(result, ct);
        }
    }
}