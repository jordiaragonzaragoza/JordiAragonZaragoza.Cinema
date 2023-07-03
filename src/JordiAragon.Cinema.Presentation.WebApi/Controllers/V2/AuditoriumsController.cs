namespace JordiAragon.Cinema.Presentation.WebApi.Controllers.V2
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Queries;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.SharedKernel.Presentation.WebApi.Controllers;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [ApiVersion("2.0", Deprecated = false)]
    public class AuditoriumsController : BaseApiController
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