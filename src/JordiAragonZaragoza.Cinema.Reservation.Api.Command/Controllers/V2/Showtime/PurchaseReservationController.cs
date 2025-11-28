namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V2.Showtime
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
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
            Summary = "Purchase a reservation for an existing Showtime. Temporal: This endpoint will not be exposed on finance integration.",
            Description = "Purchase a reservation for an existing Showtime",
            OperationId = "Showtime.PurchaseReservation.V2")
        ]
        public async Task<ActionResult> PurchaseReservationAsync(
            PurchaseReservationRequest request,
            CancellationToken cancellationToken)
        {
            var result = await this.CommandBus.SendAsync(request.ToCommand(), cancellationToken);

            return this.ToActionResult(result);
        }
    }
}