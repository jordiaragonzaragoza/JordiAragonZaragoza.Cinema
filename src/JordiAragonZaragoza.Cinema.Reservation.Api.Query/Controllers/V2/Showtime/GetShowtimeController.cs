namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V2.Showtime
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Requests;
    using Microsoft.AspNetCore.Authorization;

    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/")]
    public sealed class GetShowtimeController : BaseApiQueryController
    {
        [HttpGet(ShowtimeRoutes.GetShowtime)]
        [SwaggerOperation(
            Summary = "Get an existing showtime",
            Description = "Get an existing showtime",
            OperationId = "Showtime.GetShowtime.V2")
        ]
        public async Task<ActionResult<ShowtimeResponse>> GetShowtimeAsync(
            [FromRoute] GetShowtimeRequest request,
            CancellationToken cancellationToken)
        {
            var resultReadModels = await this.QueryBus.SendAsync(request.ToQuery(), cancellationToken);

            var resultResponse = resultReadModels.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}