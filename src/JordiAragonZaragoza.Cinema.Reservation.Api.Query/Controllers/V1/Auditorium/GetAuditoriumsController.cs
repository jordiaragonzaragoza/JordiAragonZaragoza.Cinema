namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V1.Auditorium
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1.Auditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    // TODO: Will be removed. It belongs to the cinema manager bounded context.
    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/")]
    public sealed class GetAuditoriumsController : BaseApiQueryController
    {
        [HttpGet(AuditoriumRoutes.GetAuditoriums)]
        [SwaggerOperation(
            Summary = "Gets a list of all Auditoriums. Will be removed. It belongs to the cinema manager bounded context",
            Description = "Gets a list of all Auditoriums",
            OperationId = "Auditorium.GetAuditoriums.V1")
        ]
        public async Task<ActionResult<IEnumerable<AuditoriumResponse>>> GetAuditoriumsAsync(
            CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.QueryBus.SendAsync(new GetAuditoriumsQuery(PageNumber: 1, PageSize: 1), cancellationToken);

            var resultResponse = resultOutputDto.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}