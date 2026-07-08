namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Controllers.V2.User
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/")]
    public sealed class AssignRoleController : BaseApiCommandController
    {
        [HttpPost(UserRoutes.AssignRole)]
        [SwaggerOperation(
            Summary = "Assign a role to a user in a specific scope",
            Description = "Assign a single role to a user for a specific tenant, partition, or cinema",
            OperationId = "User.AssignRole.V2")
        ]
        public async Task<ActionResult> AssignRoleAsync(
            [FromRoute] Guid userId,
            [FromBody] AssignRoleBodyRequest request,
            CancellationToken cancellationToken)
        {
            var resultResponse = await this.CommandBus.SendAsync(request.ToCommand(userId), cancellationToken);

            return this.ToActionResult(resultResponse);
        }
    }
}