namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V1.Auditorium
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1.Auditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1.Auditorium.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1.Auditorium.Responses;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("1.0", Deprecated = false)]
    public sealed class GetAvailableSeatsController : BaseApiQueryController
    {
        [HttpGet(AuditoriumRoutes.GetAvailableSeats)]
        [SwaggerOperation(
            Summary = "Gets available Seats for an existing Showtime",
            Description = "Gets available Seats for an existing Showtime",
            OperationId = "Auditorium.GetAvailableSeats.V1")
        ]
        public async Task<ActionResult<IEnumerable<SeatResponse>>> GetAvailableSeatsAsync(
            GetAvailableSeatsRequest request,
            CancellationToken cancellationToken)
        {
            var resultReadModel = await this.QueryBus.SendAsync(request.ToQuery(), cancellationToken);

            var resultResponse = resultReadModel.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}