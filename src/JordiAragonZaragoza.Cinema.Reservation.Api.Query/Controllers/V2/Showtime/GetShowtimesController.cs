namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V2.Showtime
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Requests;

    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/")]
    public sealed class GetShowtimesController : BaseApiQueryController
    {
        [HttpGet(ShowtimeRoutes.GetShowtimes)]
        [SwaggerOperation(
            Summary = "Gets a list of all Showtimes.",
            Description = "Gets a list of all Showtimes",
            OperationId = "Showtime.GetShowtimes.V2")
        ]
        public async Task<ActionResult<PaginatedCollectionResponse<ShowtimeResponse>>> GetShowtimesAsync(
            [FromQuery] GetShowtimesRequest request,
            [FromQuery] PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.QueryBus.SendAsync(request.ToQuery(paginatedRequest), cancellationToken);

            var resultResponse = resultOutputDto.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}