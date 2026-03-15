namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Movie.Tools.Queries
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Movie.Responses;
    using ApiContracts = JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using McpGatewayContracts = JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1;
    using MovieApiResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Movie.Responses.MovieResponse;

    public static class MoviesQueryMapper
    {
        public static McpGatewayContracts.Common.PaginatedCollectionResponse<MovieResponse> ToResponse(
            this ApiContracts.PaginatedCollectionResponse<MovieApiResponse> apiResponse)
        {
            return apiResponse.ToPaginatedResponse(x => x.ToResponse());
        }

        public static MovieResponse ToResponse(
            this MovieApiResponse apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return new MovieResponse(
                    apiResponse.Id,
                    apiResponse.Title,
                    apiResponse.Runtime);
        }
    }
}