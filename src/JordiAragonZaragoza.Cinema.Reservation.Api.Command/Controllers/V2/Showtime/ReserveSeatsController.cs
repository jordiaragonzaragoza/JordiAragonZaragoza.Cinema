namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V2.Showtime
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    public sealed class ReserveSeatsController : BaseVersionedApiCommandController
    {
        [HttpPost(ShowtimeRoutes.ReserveSeats)]
        [SwaggerOperation(
            Summary = "Reserve Seats for an existing Showtime",
            Description = "Reserve Seats for an existing Showtime",
            OperationId = "Showtime.ReserveSeats")
        ]
        public async Task<ActionResult<ReservationResponse>> ReserveSeatsAsync(ReserveSeatsRequest request, CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.CommandBus.SendAsync(request.ToCommand(), cancellationToken);

            var resultResponse = resultOutputDto.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}