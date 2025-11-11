namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V2.Showtime
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("2.0", Deprecated = false)]
    public sealed class GetShowtimeReservationsController : BaseApiQueryController
    {
        [HttpGet(ShowtimeRoutes.GetShowtimeReservations)]
        [SwaggerOperation(
            Summary = "Gets a list of reservations for an exiting showtime",
            Description = "Gets a list of reservations for an exiting showtime",
            OperationId = "Showtime.GetShowtimeReservations.V2")
        ]
        public async Task<ActionResult<PaginatedCollectionResponse<ReservationResponse>>> GetShowtimeReservationsAsync(
            GetShowtimeReservationsRequest request,
            CancellationToken cancellationToken)
        {
            var resultReadModels = await this.QueryBus.SendAsync(request.ToQuery(), cancellationToken);

            var resultResponse = resultReadModels.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}