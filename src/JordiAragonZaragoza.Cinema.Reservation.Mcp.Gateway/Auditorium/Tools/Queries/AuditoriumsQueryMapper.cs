namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Auditorium.Tools.Queries
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Auditorium.Responses;
    using ApiContracts = JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using AuditoriumApiResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Responses.AuditoriumResponse;
    using McpGatewayContracts = JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1;

    public static class AuditoriumsQueryMapper
    {
        public static McpGatewayContracts.Common.PaginatedCollectionResponse<AuditoriumResponse> ToResponse(
            this ApiContracts.PaginatedCollectionResponse<AuditoriumApiResponse> apiResponse)
        {
            return apiResponse.ToPaginatedResponse(x => x.ToResponse());
        }

        public static AuditoriumResponse ToResponse(
            this AuditoriumApiResponse apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return new AuditoriumResponse(
                    apiResponse.Id,
                    apiResponse.Name);
        }
    }
}