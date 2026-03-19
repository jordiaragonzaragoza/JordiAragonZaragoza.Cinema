namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.User.Tools.Queries
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.User;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.User.Tools.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2;
    using ModelContextProtocol.Server;

    [McpServerToolType]
    public sealed class GetUserReservationsTool
    {
        private readonly IReservationQueryClient apiQuery;

        public GetUserReservationsTool(IReservationQueryClient reservationApiQuery)
        {
            this.apiQuery = reservationApiQuery ?? throw new ArgumentNullException(nameof(reservationApiQuery));
        }

        [McpServerTool(Name = UserToolsNames.GetUserReservations, Title = "Get User Reservations")]
        [Description(
        """
        Gets a list of reservations for an exiting user.
        """)]
        public async Task<PaginatedCollectionResponse<ReservationResponse>> GetUserReservationsAsync(
            [Description("The user reservations request parameters.")]
            UserReservationsRequest request,
            [Description("The pagination parameters.")]
            PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken)
            {
                var apiResponse = await this.apiQuery.GetUserReservationsAsync(request.ToApiRequest(), paginatedRequest.ToApiRequest(), cancellationToken);

                return apiResponse.ToResponse();
            }
    }
}