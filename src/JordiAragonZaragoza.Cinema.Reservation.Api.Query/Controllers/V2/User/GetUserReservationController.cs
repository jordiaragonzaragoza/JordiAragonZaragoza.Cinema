namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V2.User
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("2.0", Deprecated = false)]
    public sealed class GetUserReservationController : BaseApiQueryController
    {
        [HttpGet(UserRoutes.GetUserReservation)]
        [SwaggerOperation(
            Summary = "Gets a user reservation for existing showtime",
            Description = "Gets a user reservation for existing showtime",
            OperationId = "User.GetUserReservation.V2")
        ]
        public async Task<ActionResult<ReservationResponse>> GetUserReservationAsync(
            UserReservationRequest request,
            CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.QueryBus.SendAsync(request.ToQuery(), cancellationToken);

            var resultResponse = resultOutputDto.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}