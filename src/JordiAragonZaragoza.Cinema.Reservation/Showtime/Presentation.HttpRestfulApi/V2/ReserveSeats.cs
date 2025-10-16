namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    public sealed class ReserveSeats : Endpoint<ReserveSeatsRequest, ReservationResponse>
    {
        private readonly ICommandBus commandBus;

        public ReserveSeats(ICommandBus commandBus)
        {
            this.commandBus = Guard.Against.Null(commandBus, nameof(commandBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Put(ReserveSeatsRequest.Route);
            this.Version(2);
            this.Summary(summary =>
            {
                summary.Summary = "Reserve Seats for an existing Showtime";
                summary.Description = "Reserve Seats for an existing Showtime";
            });
        }

        public async override Task HandleAsync(ReserveSeatsRequest req, CancellationToken ct)
        {
            var command = req.ToCommand();

            var resultOutputDto = await this.commandBus.SendAsync(command, ct);

            var resultResponse = resultOutputDto.ToResponse();

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}