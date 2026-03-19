namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.User.Tools.Queries
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.User.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2;
    using ModelContextProtocol.Server;

    // TODO: Will be moved. It belongs to the management bounded context.
    [McpServerToolType]
    public sealed class GetUsersTool
    {
        private readonly IReservationQueryClient apiQuery;

        public GetUsersTool(IReservationQueryClient reservationApiQuery)
        {
            this.apiQuery = reservationApiQuery ?? throw new ArgumentNullException(nameof(reservationApiQuery));
        }

        [McpServerTool(Name = "get_users", Title = "Get Users")]
        [Description(
        """
        Gets a list of all users.
        """)]
        public async Task<PaginatedCollectionResponse<UserResponse>> GetUsersAsync(
            [Description("The pagination parameters.")]
            PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken)
            {
                var apiResponse = await this.apiQuery.GetUsersAsync(paginatedRequest.ToApiRequest(), cancellationToken);

                return apiResponse.ToResponse();
            }
    }
}