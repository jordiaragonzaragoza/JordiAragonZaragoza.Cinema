namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V2.Movie
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Movie;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Movie.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    // TODO: It belongs to the catalog bounded context.
    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}/")]
    public sealed class GetMovies : BaseApiQueryController
    {
        [HttpGet(MovieRoutes.GetMovies)]
        [SwaggerOperation(
            Summary = "Gets a list of all Movies. Will be removed. It belongs to the catalog bounded context",
            Description = "Gets a list of all Movies",
            OperationId = "Movie.GetMovies.V2")
        ]
        public async Task<ActionResult<PaginatedCollectionResponse<MovieResponse>>> GetMoviesAsync(
            [FromQuery] PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(paginatedRequest);

            var query = new GetMoviesQuery(
                paginatedRequest.PageNumber,
                paginatedRequest.PageSize);

            var resultOutputDto = await this.QueryBus.SendAsync(query, cancellationToken);

            var resultResponse = resultOutputDto.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}