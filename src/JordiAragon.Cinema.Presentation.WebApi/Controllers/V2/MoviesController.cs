namespace JordiAragon.Cinema.Presentation.WebApi.Controllers.V2
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.Cinema.Application.Contracts.Features.Movie.Queries;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Movie.Responses;
    using JordiAragon.SharedKernel.Presentation.WebApi.Controllers;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [ApiVersion("2.0", Deprecated = false)]
    public class MoviesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieResponse>>> GetAsync(CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.Sender.Send(new GetMoviesQuery(), cancellationToken);

            var resultResponse = this.Mapper.Map<Result<IEnumerable<MovieResponse>>>(resultOutputDto);

            return this.ToActionResult(resultResponse);
        }
    }
}