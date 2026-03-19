namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Movie.Tools.Queries
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Movie.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2;
    using ModelContextProtocol.Server;

    // TODO: Will be moved. It belongs to the catalog bounded context.
    [McpServerToolType]
    public sealed class GetMoviesTool
    {
        private readonly IReservationQueryClient apiQuery;

        public GetMoviesTool(IReservationQueryClient reservationApiQuery)
        {
            this.apiQuery = reservationApiQuery ?? throw new ArgumentNullException(nameof(reservationApiQuery));
        }

        [McpServerTool(Name = "get_movies", Title = "Get Movies")]
        [Description(
        """
        Gets a list of all movies.
        """)]
        public async Task<PaginatedCollectionResponse<MovieResponse>> GetMoviesAsync(
            [Description("The pagination parameters.")]
            PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken)
            {
                var apiResponse = await this.apiQuery.GetMoviesAsync(paginatedRequest.ToApiRequest(), cancellationToken);

                return apiResponse.ToResponse();
            }
    }
}