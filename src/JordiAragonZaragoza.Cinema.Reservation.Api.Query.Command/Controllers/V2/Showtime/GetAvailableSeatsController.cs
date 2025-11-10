namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V2.Showtime
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("2.0", Deprecated = false)]
    public sealed class GetAvailableSeatsController : BaseApiQueryController
    {
        [HttpGet(ShowtimeRoutes.GetAvailableSeats)]
        [SwaggerOperation(
            Summary = "Gets available Seats for an existing Showtime",
            Description = "Gets available Seats for an existing Showtime",
            OperationId = "Showtime.GetAvailableSeats.V2")
        ]
        public async Task<ActionResult<IEnumerable<SeatResponse>>> GetAvailableSeatsAsync(
            GetAvailableSeatsRequest request,
            CancellationToken cancellationToken)
        {
            var resultReadModels = await this.QueryBus.SendAsync(request.ToQuery(), cancellationToken);

            var resultResponse = resultReadModels.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}