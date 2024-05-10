namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using MediatR;

    public sealed class CancelShowtime : Endpoint<CancelShowtimeRequest>
    {
        public const string Route = "showtimes/{showtimeId}";

        private readonly ISender internalBus;

        public CancelShowtime(ISender internalBus)
        {
            this.internalBus = Guard.Against.Null(internalBus, nameof(internalBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Delete(CancelShowtime.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Cancels a scheduled Showtime";
                summary.Description = "Cancels a scheduled Showtime";
            });
        }

        public async override Task HandleAsync(CancelShowtimeRequest req, CancellationToken ct)
        {
            var resultResponse = await this.internalBus.Send(new CancelShowtimeCommand(req.ShowtimeId), ct);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}