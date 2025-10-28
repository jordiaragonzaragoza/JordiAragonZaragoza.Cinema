namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V1.Auditorium
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium.Requests;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    public sealed class PurchaseReservationController : BaseVersionedApiCommandController
    {
        [HttpPatch(AuditoriumRoutes.PurchaseReservation)]
        [SwaggerOperation(
            Summary = "Purchase a reservation for an existing Showtime. Temporal: This endpoint will not be exposed on finance integration.",
            Description = "Purchase a reservation for an existing Showtime",
            OperationId = "Auditorium.PurchaseReservation")
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