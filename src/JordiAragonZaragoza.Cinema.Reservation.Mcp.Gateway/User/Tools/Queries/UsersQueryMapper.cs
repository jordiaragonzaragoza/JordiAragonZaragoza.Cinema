namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.User.Tools.Queries
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.User.Responses;
    using ApiContracts = JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using McpGatewayContracts = JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1;
    using UserApiResponse = JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Responses.UserResponse;

    public static class UsersQueryMapper
    {
        public static McpGatewayContracts.Common.PaginatedCollectionResponse<UserResponse> ToResponse(
            this ApiContracts.PaginatedCollectionResponse<UserApiResponse> apiResponse)
        {
            return apiResponse.ToPaginatedResponse(x => x.ToResponse());
        }

        public static UserResponse ToResponse(
            this UserApiResponse apiResponse)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);

            return new UserResponse(
                    apiResponse.Id);
        }
    }
}