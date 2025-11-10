namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Controllers.V2.Movie
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Movie;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Movie.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Movie.Responses;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Controllers;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    // TODO: It belongs to the catalog bounded context.
    [AllowAnonymous] // TODO: Temporal. Remove when authentication is implemented.
    [Asp.Versioning.ApiVersion("2.0", Deprecated = false)]
    public sealed class GetMovies : BaseApiQueryController
    {
        [HttpGet(MovieRoutes.GetMovies)]
        [SwaggerOperation(
            Summary = "Gets a list of all Movies. Will be removed. It belongs to the catalog bounded context",
            Description = "Gets a list of all Movies",
            OperationId = "Movie.GetMovies.V2")
        ]
        public async Task<ActionResult<PaginatedCollectionResponse<MovieResponse>>> GetMoviesAsync(
            GetMoviesRequest request,
            CancellationToken cancellationToken)
        {
            var resultOutputDto = await this.QueryBus.SendAsync(request.ToQuery(), cancellationToken);

            var resultResponse = resultOutputDto.ToResponse();

            return this.ToActionResult(resultResponse);
        }
    }
}