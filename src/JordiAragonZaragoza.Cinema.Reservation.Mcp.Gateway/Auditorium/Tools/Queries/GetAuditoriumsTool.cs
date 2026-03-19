namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Auditorium.Tools.Queries
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Sdk.Query.V2;
    using ModelContextProtocol.Server;

    // TODO: Will be moved. It belongs to the cinema manager bounded context.
    [McpServerToolType]
    public sealed class GetAuditoriumsTool
    {
        private readonly IReservationQueryClient apiQuery;

        public GetAuditoriumsTool(IReservationQueryClient reservationApiQuery)
        {
            this.apiQuery = reservationApiQuery ?? throw new ArgumentNullException(nameof(reservationApiQuery));
        }

        [McpServerTool(Name = "get_auditoriums", Title = "Get Auditoriums")]
        [Description(
        """
        Gets a list of all auditoriums.
        """)]
        public async Task<PaginatedCollectionResponse<AuditoriumResponse>> GetAuditoriumsAsync(
            [Description("The pagination parameters.")]
            PaginatedRequest paginatedRequest,
            CancellationToken cancellationToken)
            {
                var apiResponse = await this.apiQuery.GetAuditoriumsAsync(paginatedRequest.ToApiRequest(), cancellationToken);

                return apiResponse.ToResponse();
            }
    }
}