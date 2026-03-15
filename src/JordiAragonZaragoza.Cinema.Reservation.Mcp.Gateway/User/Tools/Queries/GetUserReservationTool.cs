namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.User.Tools.Queries
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2;
    using ModelContextProtocol.Server;

    [McpServerToolType]
    public sealed class GetUserReservationTool
    {
        private readonly IReservationQueryClient apiQuery;

        public GetUserReservationTool(IReservationQueryClient reservationApiQuery)
        {
            this.apiQuery = reservationApiQuery ?? throw new ArgumentNullException(nameof(reservationApiQuery));
        }

        [McpServerTool(Name = "get_user_reservation", Title = "Get User Reservation")]
        [Description(
        """
        Gets a user reservation for existing showtime.
        """)]
        public async Task<ReservationResponse> GetUserReservationsAsync(
            [Description("The user reservation request parameters.")]
            UserReservationRequest request,
            CancellationToken cancellationToken)
            {
                var apiResponse = await this.apiQuery.GetUserReservationAsync(request.ToApiRequest(), cancellationToken);

                return apiResponse.ToResponse();
            }
    }
}