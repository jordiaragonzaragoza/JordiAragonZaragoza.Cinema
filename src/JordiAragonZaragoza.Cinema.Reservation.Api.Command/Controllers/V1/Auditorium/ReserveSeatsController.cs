namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V1.Auditorium
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V1;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    public sealed class ReserveSeatsController : BaseVersionedApiCommandController
    {
        [HttpPost(AuditoriumRoutes.ReserveSeats)]
        [SwaggerOperation(
            Summary = "Reserve Seats for an existing Showtime",
            Description = "Reserve Seats for an existing Showtime",
            OperationId = "Auditorium.ReserveSeats")
        ]
        public async Task<ActionResult<ReservationResponse>> ReserveSeatsAsync(ReserveSeatsRequest request, CancellationToken cancellationToken)
        {
            // This generated Id is done here to support compatibility with the current implementation which client provides the Id.
            var reservationId = Guid.NewGuid();

            var resultOutputDto = await this.CommandBus.SendAsync(request.ToCommand(reservationId), cancellationToken);

            var resultResponse = resultOutputDto.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}