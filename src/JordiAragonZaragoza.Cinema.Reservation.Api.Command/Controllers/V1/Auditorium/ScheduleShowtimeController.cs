namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V1.Auditorium
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium.Requests;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    public sealed class ScheduleShowtimeController : BaseVersionedApiCommandController
    {
        [HttpPost(AuditoriumRoutes.ScheduleShowtime)]
        [SwaggerOperation(
            Summary = "Schedule a new Showtime",
            Description = "Schedule a new Showtime",
            OperationId = "Auditorium.ScheduleShowtime")
        ]
        public async Task<ActionResult<Guid>> ScheduleShowtimeAsync(ScheduleShowtimeRequest request, CancellationToken cancellationToken)
        {
            // This generated Id is done here to support compatibility with the current implementation which client provides the Id.
            var showtimeId = Guid.NewGuid();

            var resultResponse = await this.CommandBus.SendAsync(request.ToCommand(showtimeId), cancellationToken);

            if (resultResponse.IsSuccess)
            {
                return this.ToActionResult(Result.Created(showtimeId));
            }

            return this.ToActionResult(resultResponse);
        }
    }
}