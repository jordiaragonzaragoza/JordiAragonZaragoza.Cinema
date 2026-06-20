namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V2.User
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Responses;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/")]
    public sealed class GetUserAuthorizationsController : BaseApiQueryController
    {
        [HttpGet(UserRoutes.GetUserAuthorizations)]
        [SwaggerOperation(
            Summary = "Gets a user authorizations for the requested scope",
            Description = "Gets a user authorizations for the requested scope",
            OperationId = "User.GetUserAuthorization.V2")]
        public async Task<ActionResult<IReadOnlyCollection<UserAuthorizationResponse>>> GetUserAuthorizationAsync(
            [FromRoute] UserAuthorizationRequest request,
            CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.QueryBus.SendAsync(request.ToQuery(), cancellationToken);

            var resultResponse = resultOutputDto.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}
