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
    public sealed class CancelShowtimeController : BaseApiCommandController
    {
        [HttpDelete(ShowtimeRoutes.CancelShowtime)]
        [SwaggerOperation(
            Summary = "Cancels a scheduled Showtime",
            Description = "Cancels a scheduled Showtime",
            OperationId = "Showtime.CancelShowtime.V2")
        ]
        public async Task<ActionResult> CancelShowtimeAsync([FromRoute] Guid showtimeId, CancellationToken ct)
        {
            var resultResponse = await this.CommandBus.SendAsync(new CancelShowtimeCommand(showtimeId), ct);

            return this.ToActionResult(resultResponse);
        }
    }
}