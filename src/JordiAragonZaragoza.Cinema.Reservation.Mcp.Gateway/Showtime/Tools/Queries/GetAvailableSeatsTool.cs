namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Showtime.Tools.Queries
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2;
    using ModelContextProtocol.Server;

    [McpServerToolType]
    public sealed class GetAvailableSeatsTool
    {
        private readonly IReservationQueryClient apiQuery;

        public GetAvailableSeatsTool(IReservationQueryClient reservationApiQuery)
        {
            this.apiQuery = reservationApiQuery ?? throw new ArgumentNullException(nameof(reservationApiQuery));
        }

        [McpServerTool(Name = ShowtimeToolsNames.GetAvailableSeats, Title = "Get Available Seats")]
        [Description(
        """
        Get the list of available seats for a specific existing showtime.
        """)]
        public async Task<IEnumerable<SeatResponse>> GetAvailableSeatsAsync(
            [Description("The Id of the showtime for which to get available seats.")]
            Guid showtimeId,
            CancellationToken cancellationToken)
            {
                var apiResponse = await this.apiQuery.GetAvailableSeatsAsync(showtimeId, cancellationToken);

                return apiResponse.ToResponse();
            }
    }
}