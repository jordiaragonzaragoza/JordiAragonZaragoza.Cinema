namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Showtime.Tools
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2;
    using ModelContextProtocol.Server;

    [McpServerToolType]
    public sealed class GetShowtimeReservationTool
    {
        private readonly IReservationQueryClient apiQuery;

        public GetShowtimeReservationTool(IReservationQueryClient reservationApiQuery)
        {
            this.apiQuery = reservationApiQuery ?? throw new ArgumentNullException(nameof(reservationApiQuery));
        }

        [McpServerTool(Name = "get_showtime_reservation", Title = "Get Showtime Reservation")]
        [Description(
        """
        Get an existing showtime reservation.
        """)]
        public async Task<ReservationResponse> GetShowtimeReservationAsync(
            [Description("The showtime reservation identifier.")]
            Guid reservationId,
            CancellationToken cancellationToken)
            {
                var apiResponse = await this.apiQuery.GetShowtimeReservationAsync(reservationId, cancellationToken);

                return apiResponse.ToResponse();
            }
    }
}