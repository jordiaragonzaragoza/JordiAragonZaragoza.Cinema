namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Presentation.HttpRestfulApi.V1
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FastEndpoints;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Reservation.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Reservation.Responses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;

    public sealed class ReserveSeats : Endpoint<ReserveSeatsRequest, ReservationResponse>
    {
        private readonly ICommandBus commandBus;

        public ReserveSeats(ICommandBus commandBus)
        {
            this.commandBus = commandBus ?? throw new ArgumentNullException(nameof(commandBus));
        }

        public override void Configure()
        {
            this.AllowAnonymous();
            this.Post("auditoriums/{auditoriumId}/showtimes/{showtimeId}/reservations");
            this.Version(1, 2);
            this.Summary(summary =>
            {
                summary.Summary = "Reserve Seats for an existing Showtime";
                summary.Description = "Reserve Seats for an existing Showtime";
            });
        }

        public async override Task HandleAsync(ReserveSeatsRequest req, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(req, nameof(req));

            // This generated Id is done here to support compatibility with the current implementation which client provides the Id.
            var reservationId = Guid.NewGuid();

            var resultOutputDto = await this.commandBus.SendAsync(req.ToCommand(reservationId), ct);

            var resultResponse = resultOutputDto.ToResponse();

            await this.SendResponseAsync(resultResponse, ct);
        }
    }
}