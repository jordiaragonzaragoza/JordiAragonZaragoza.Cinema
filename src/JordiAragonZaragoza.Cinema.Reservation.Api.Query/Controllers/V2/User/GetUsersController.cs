namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V2.User
{
    using System;

    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    // TODO: It belongs to the management bounded context.
    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/")]
    public sealed class GetUsersController : BaseApiQueryController
    {
        [HttpGet(UserRoutes.GetUsers)]
        [SwaggerOperation(
            Summary = "Gets a list of all Users. Temporal: It belongs to the management bounded context.",
            Description = "Gets a list of all Users",
            OperationId = "User.GetUsers.V2")
        ]
        public async Task<ActionResult<PaginatedCollectionResponse<UserResponse>>> GetUsersAsync(
            [FromQuery] PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(paginatedRequest);

            var query = new GetUsersQuery(
                paginatedRequest.PageNumber,
                paginatedRequest.PageSize);

            var resultOutputDto = await this.QueryBus.SendAsync(query, cancellationToken);

            var resultResponse = resultOutputDto.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}