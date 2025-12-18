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
    public sealed class PurchaseReservationController : BaseApiCommandController
    {
        [HttpPatch(ShowtimeRoutes.PurchaseReservation)]
        [SwaggerOperation(
            Summary = "Purchase a reservation for an existing Showtime. Temporal: This endpoint will not be exposed on payment integration. It will be implemented using integration events.",
            Description = "Purchase a reservation for an existing Showtime",
            OperationId = "Showtime.PurchaseReservation.V2")
        ]
        public async Task<ActionResult> PurchaseReservationAsync(
            [FromRoute] Guid showtimeId,
            [FromRoute] Guid reservationId,
            CancellationToken cancellationToken)
        {
            var command = new PurchaseReservationCommand(
                showtimeId,
                reservationId);

            var result = await this.CommandBus.SendAsync(command, cancellationToken);

            return this.ToActionResult(result);
        }
    }
}