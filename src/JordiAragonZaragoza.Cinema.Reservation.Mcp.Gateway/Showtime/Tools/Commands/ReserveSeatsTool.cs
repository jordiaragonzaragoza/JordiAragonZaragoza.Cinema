namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Showtime.Tools.Commands
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Command.V2;
    using ModelContextProtocol.Server;

    [McpServerToolType]
    public sealed class ReserveSeatsTool
    {
        private readonly IReservationCommandClient apiCommand;

        public ReserveSeatsTool(IReservationCommandClient reservationApiQuery)
        {
            this.apiCommand = reservationApiQuery ?? throw new ArgumentNullException(nameof(reservationApiQuery));
        }

        [McpServerTool(Name = ShowtimeToolsNames.ReserveSeats, Title = "Reserve Seats")]
        [Description(
        """
        Reserves seats for an existing showtime.

        Use this tool when seats need to be reserved for a specific showtime.
        """)]
        public async Task<ReservationResponse> ReserveSeatsAsync(
            [Description("The Id of the showtime for which to reserve seats.")]
            Guid showtimeId,
            [Description("The Id of the reservation.")]
            Guid reservationId,
            [Description("The details of the seats to reserve.")]
            ReserveSeatsBodyRequest request,
            CancellationToken cancellationToken)
            {
                var apiResponse = await this.apiCommand.ReserveSeatsAsync(reservationId, showtimeId, request.ToApiRequest(), cancellationToken);

                return apiResponse.ToResponse();
            }
    }
}