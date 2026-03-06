namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Showtime.Tools
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Infrastructure.Api.Command;
    using ModelContextProtocol.Server;

    [McpServerToolType]
    public sealed class CancelShowtimeTool
    {
        private readonly CommandService commandService;

        public CancelShowtimeTool(CommandService reservationApiQuery)
        {
            this.commandService = reservationApiQuery ?? throw new ArgumentNullException(nameof(reservationApiQuery));
        }

        [McpServerTool(Name = "CancelShowtime")]
        [Description("Cancels a scheduled Showtime.")]
        public Task CancelShowtimeAsync(
            [Description("The Id of the showtime to cancel")]
            Guid showtimeId,
            CancellationToken cancellationToken)
            => this.commandService.CancelShowtimeAsync(showtimeId, cancellationToken);
    }
}