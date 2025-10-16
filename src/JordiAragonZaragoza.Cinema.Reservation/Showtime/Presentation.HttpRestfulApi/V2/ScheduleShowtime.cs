namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    public sealed class ScheduleShowtime : Endpoint<ScheduleShowtimeRequest>
    {
        private readonly ICommandBus commandBus;

        public ScheduleShowtime(ICommandBus commandBus)
        {
            this.commandBus = Guard.Against.Null(commandBus, nameof(commandBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Put(ScheduleShowtimeRequest.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Schedule a new Showtime";
                summary.Description = "Schedule a new Showtime";
            });
        }

        public async override Task HandleAsync(ScheduleShowtimeRequest req, CancellationToken ct)
        {
            var resultResponse = await this.commandBus.SendAsync(req.ToCommand(), ct);

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}