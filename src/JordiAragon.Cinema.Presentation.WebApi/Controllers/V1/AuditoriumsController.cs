namespace JordiAragon.Cinema.Presentation.WebApi.Controllers.V1
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Queries;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V1.Auditorium.Responses;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using Microsoft.AspNetCore.Mvc;

    public class AuditoriumsController : BaseVersionedApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditoriumResponse>>> GetAsync(CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.Sender.Send(new GetAuditoriumsQuery(), cancellationToken);

            var resultResponse = this.Mapper.Map<Result<IEnumerable<AuditoriumResponse>>>(resultOutputDto);

            return this.ToActionResult(resultResponse);
        }
    }
}