namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V2.Showtime
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/")]
    public sealed class CancelReservationController : BaseApiCommandController
    {
        [HttpDelete(ShowtimeRoutes.CancelReservation)]
        [SwaggerOperation(
            Summary = "Cancels a reservation for a scheduled Showtime",
            Description = "Cancels a reservation for a scheduled showtime.",
            OperationId = "Showtime.CancelReservation.V2")
        ]
        public async Task<ActionResult> CancelReservationAsync(
            [FromRoute] Guid showtimeId,
            [FromRoute] Guid reservationId,
            CancellationToken cancellationToken)
        {
            var resultResponse = await this.CommandBus.SendAsync(new CancelReservationCommand(showtimeId, reservationId), cancellationToken);

            return this.ToActionResult(resultResponse);
        }
    }
}