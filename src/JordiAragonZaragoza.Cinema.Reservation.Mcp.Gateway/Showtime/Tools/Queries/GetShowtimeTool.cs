namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Showtime.Tools.Queries
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2;
    using ModelContextProtocol.Server;

    [McpServerToolType]
    public sealed class GetShowtimeTool
    {
        private readonly IReservationQueryClient apiQuery;

        public GetShowtimeTool(IReservationQueryClient reservationApiQuery)
        {
            this.apiQuery = reservationApiQuery ?? throw new ArgumentNullException(nameof(reservationApiQuery));
        }

        [McpServerTool(Name = "get_showtime", Title = "Get Showtime")]
        [Description(
        """
        Get the details of a specific existing showtime, including the movie title, session date, auditorium information, and available seats.
        """)]
        public async Task<ShowtimeResponse> GetShowtimeAsync(
            [Description("The Id of the showtime for which to get details.")]
            Guid showtimeId,
            CancellationToken cancellationToken)
            {
                var apiResponse = await this.apiQuery.GetShowtimeAsync(showtimeId, cancellationToken);

                return apiResponse.ToResponse();
            }
    }
}