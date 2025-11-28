namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V1.Auditorium
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium.Responses;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/")]
    public sealed class ReserveSeatsController : BaseApiCommandController
    {
        [HttpPost(AuditoriumRoutes.ReserveSeats)]
        [SwaggerOperation(
            Summary = "Reserve Seats for an existing Showtime",
            Description = "Reserve Seats for an existing Showtime",
            OperationId = "Auditorium.ReserveSeats.V1")
        ]
        public async Task<ActionResult<ReservationResponse>> ReserveSeatsAsync(ReserveSeatsRequest request, CancellationToken cancellationToken)
        {
            // This generated Id is done here to support compatibility with the current implementation which client provides the Id.
            var reservationId = Guid.NewGuid();

            var resultOutputDto = await this.CommandBus.SendAsync(request.ToCommand(reservationId), cancellationToken);

            var resultResponse = resultOutputDto.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}