namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V2.Showtime
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/")]
    public sealed class ReserveSeatsController : BaseApiCommandController
    {
        [HttpPut(ShowtimeRoutes.ReserveSeats)]
        [SwaggerOperation(
            Summary = "Reserve Seats for an existing Showtime",
            Description = "Reserve Seats for an existing Showtime",
            OperationId = "Showtime.ReserveSeats.V2")
        ]
        public async Task<ActionResult<ReservationResponse>> ReserveSeatsAsync(ReserveSeatsRequest request, CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.CommandBus.SendAsync(request.ToCommand(), cancellationToken);

            var resultResponse = resultOutputDto.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}