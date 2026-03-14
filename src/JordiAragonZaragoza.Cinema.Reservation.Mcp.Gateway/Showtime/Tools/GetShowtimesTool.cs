namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Showtime.Tools
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2;
    using ModelContextProtocol.Server;

    [McpServerToolType]
    public sealed class GetShowtimesTool
    {
        private readonly IReservationQueryClient apiQuery;

        public GetShowtimesTool(IReservationQueryClient reservationApiQuery)
        {
            this.apiQuery = reservationApiQuery ?? throw new ArgumentNullException(nameof(reservationApiQuery));
        }

        [McpServerTool(Name = "get_showtimes", Title = "Get Showtimes")]
        [Description(
        """
        Gets a list of all showtimes.
        """)]
        public async Task<PaginatedCollectionResponse<ShowtimeResponse>> GetShowtimesAsync(
            [Description("The request with the filters to get the showtimes.")]
            GetShowtimesRequest request,
            [Description("The pagination parameters.")]
            PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken)
            {
                var apiResponse = await this.apiQuery.GetShowtimesAsync(request.ToApiRequest(), paginatedRequest.ToApiRequest(), cancellationToken);

                return apiResponse.ToResponse();
            }
    }
}