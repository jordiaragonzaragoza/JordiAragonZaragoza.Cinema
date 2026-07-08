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
    public sealed class RemovePermissionController : BaseApiCommandController
    {
        [HttpDelete(UserRoutes.RemovePermission)]
        [SwaggerOperation(
            Summary = "Remove a permission from a user in a specific scope",
            Description = "Remove a single permission from a user for a specific tenant, partition, or cinema",
            OperationId = "User.RemovePermission.V2")
        ]
        public async Task<ActionResult> RemovePermissionAsync(
            [FromRoute] Guid userId,
            [FromBody] RemovePermissionBodyRequest request,
            CancellationToken cancellationToken)
        {
            var resultResponse = await this.CommandBus.SendAsync(request.ToCommand(userId), cancellationToken);

            return this.ToActionResult(resultResponse);
        }
    }
}
