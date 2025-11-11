namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V1.Movie
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1.Movie;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1.Movie.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    // TODO: It belongs to the catalog bounded context.
    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("1.0", Deprecated = false)]
    public sealed class GetMovies : BaseApiQueryController
    {
        [HttpGet(MovieRoutes.GetMovies)]
        [SwaggerOperation(
            Summary = "Gets a list of all Movies. Will be removed. It belongs to the catalog bounded context",
            Description = "Gets a list of all Movies",
            OperationId = "Movie.GetMovies.V1")
        ]
        public async Task<ActionResult<IEnumerable<MovieResponse>>> GetMoviesAsync(
            CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.QueryBus.SendAsync(new GetMoviesQuery(PageNumber: 1, PageSize: 1), cancellationToken);

            var resultResponse = resultOutputDto.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}