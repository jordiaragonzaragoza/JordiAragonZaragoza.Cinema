namespace JordiAragon.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    public sealed class CancelShowtime : Endpoint<CancelShowtimeRequest>
    {
        public const string Route = "showtimes/{showtimeId}";

        private readonly ICommandBus commandBus;

        public CancelShowtime(ICommandBus queryBus)
        {
            this.commandBus = Guard.Against.Null(queryBus, nameof(queryBus));
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
            var resultResponse = await this.commandBus.SendAsync(new CancelShowtimeCommand(req.ShowtimeId), ct);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}