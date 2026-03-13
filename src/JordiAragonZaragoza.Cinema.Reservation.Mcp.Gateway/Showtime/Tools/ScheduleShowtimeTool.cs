namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Showtime.Tools
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Command.V2;
    using ModelContextProtocol.Server;

    [McpServerToolType]
    public sealed class ScheduleShowtimeTool
    {
        private readonly IReservationCommandClient apiCommand;

        public ScheduleShowtimeTool(IReservationCommandClient reservationApiQuery)
        {
            this.apiCommand = reservationApiQuery ?? throw new ArgumentNullException(nameof(reservationApiQuery));
        }

        [McpServerTool(Name = "schedule_showtime", Title = "Schedule Showtime")]
        [Description(
        """
        Schedules a new showtime.

        Use this tool when a showtime must be scheduled for a future date.
        """)]
        public async Task<string> ScheduleShowtimeAsync(
            [Description("The Id of the showtime to schedule.")]
            Guid showtimeId,
            [Description("The details of the showtime to schedule.")]
            ScheduleShowtimeBodyRequest request,
            CancellationToken cancellationToken)
            {
                await this.apiCommand.ScheduleShowtimeAsync(showtimeId, request.ToApiRequest(), cancellationToken);

                return $"Showtime {showtimeId} scheduled successfully.";
            }
    }
}