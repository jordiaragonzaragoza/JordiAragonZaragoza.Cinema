namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Controllers.V2
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Movie.Queries;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Movie.Responses;
    using JordiAragon.SharedKernel.Presentation.WebApi.Controllers;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    [AllowAnonymous]
    [ApiVersion("2.0", Deprecated = false)]
    public class MoviesController : BaseApiController
    {
        [HttpGet]
        [SwaggerOperation(
            Summary = "Gets a list of all Movies",
            Description = "Gets a list of all Movies",
            OperationId = "Movie.List")
        ]
        public async Task<ActionResult<IEnumerable<MovieResponse>>> GetAsync(CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.Sender.Send(new GetMoviesQuery(), cancellationToken);

            var resultResponse = this.Mapper.Map<Result<IEnumerable<MovieResponse>>>(resultOutputDto);

            return this.ToActionResult(resultResponse);
        }
    }
}