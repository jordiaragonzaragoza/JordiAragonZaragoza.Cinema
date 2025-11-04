namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V2.Showtime
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    public sealed class ScheduleShowtimeController : BaseVersionedApiCommandController
    {
        [HttpPost(ShowtimeRoutes.ScheduleShowtime)]
        [SwaggerOperation(
            Summary = "Schedule a new Showtime",
            Description = "Schedule a new Showtime",
            OperationId = "Showtime.ScheduleShowtime")
        ]
        public async Task<ActionResult<Guid>> ScheduleShowtimeAsync(ScheduleShowtimeRequest request, CancellationToken cancellationToken)
        {
            var resultResponse = await this.CommandBus.SendAsync(request.ToCommand(), cancellationToken);

            return this.ToActionResult(resultResponse);
        }
    }
}