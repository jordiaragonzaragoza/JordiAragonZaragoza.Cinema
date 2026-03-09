namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Showtime.Tools
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Command.V2;

    using ModelContextProtocol.Server;

    [McpServerToolType]
    public sealed class CancelShowtimeTool
    {
        private readonly IReservationCommandClient apiCommand;

        public CancelShowtimeTool(IReservationCommandClient reservationApiQuery)
        {
            this.apiCommand = reservationApiQuery ?? throw new ArgumentNullException(nameof(reservationApiQuery));
        }

        [McpServerTool(Name = "cancel_showtime")]
        [Description(
        """
        Cancels a scheduled showtime.

        Use this tool when a showtime must be cancelled due to operational reasons.
        Once cancelled, no new reservations can be created for the showtime.
        """)]
        public async Task<string> CancelShowtimeAsync(
            [Description("The Id of the showtime to cancel")]
            Guid showtimeId,
            CancellationToken cancellationToken)
            {
                await this.apiCommand.CancelShowtimeAsync(showtimeId, cancellationToken);

                return $"Showtime {showtimeId} cancelled successfully.";
            }
    }
}