namespace JordiAragon.Cinema.Presentation.WebApi.Controllers.V1
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.Cinema.Application.Contracts.Features.Movie.Queries;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V1.Movie.Responses;
    using JordiAragon.SharedKernel.Presentation.WebApi.Helpers;
    using Microsoft.AspNetCore.Mvc;

    public class MoviesController : BaseVersionedApiController
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