namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V2.Showtime
{
    using System;
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
    public sealed class ScheduleShowtimeController : BaseApiCommandController
    {
        [HttpPut(ShowtimeRoutes.ScheduleShowtime)]
        [SwaggerOperation(
            Summary = "Schedule a new Showtime",
            Description = "Schedule a new Showtime",
            OperationId = "Showtime.ScheduleShowtime.V2")
        ]
        public async Task<ActionResult> ScheduleShowtimeAsync(
            [FromRoute] Guid showtimeId,
            [FromBody] ScheduleShowtimeBodyRequest request,
            CancellationToken cancellationToken)
        {
            var resultResponse = await this.CommandBus.SendAsync(request.ToCommand(showtimeId), cancellationToken);

            return this.ToActionResult(resultResponse);
        }
    }
}