namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V2.Auditorium
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    // TODO: Will be removed. It belongs to the cinema manager bounded context.
    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("2.0", Deprecated = false)]
    public sealed class GetAuditoriumsController : BaseApiQueryController
    {
        [HttpGet(AuditoriumRoutes.GetAuditoriums)]
        [SwaggerOperation(
            Summary = "Gets a list of all Auditoriums. Will be removed. It belongs to the cinema manager bounded context",
            Description = "Gets a list of all Auditoriums",
            OperationId = "Auditorium.GetAuditoriums.V2")
        ]
        public async Task<ActionResult<PaginatedCollectionResponse<AuditoriumResponse>>> GetAuditoriumsAsync(
            GetAuditoriumsRequest request,
            CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.QueryBus.SendAsync(request.ToQuery(), cancellationToken);

            var resultResponse = resultOutputDto.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}