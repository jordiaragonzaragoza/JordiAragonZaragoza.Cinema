namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Common
{
    using System;
    using System.Collections.Generic;
    using ApiContracts = JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using McpGatewayContracts = JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1;

    public static class CommonMappers
    {
        public static ApiContracts.PaginatedRequest ToApiRequest(
            this McpGatewayContracts.Common.PaginatedRequest paginatedRequest)
        {
            ArgumentNullException.ThrowIfNull(paginatedRequest);

            return new ApiContracts.PaginatedRequest()
            {
                PageNumber = paginatedRequest.PageNumber,
                PageSize = paginatedRequest.PageSize,
            };
        }

        public static McpGatewayContracts.Common.PaginatedCollectionResponse<TDestination> ToPaginatedResponse<TSource, TDestination>(
            this ApiContracts.PaginatedCollectionResponse<TSource> apiResponse,
            Func<TSource, TDestination> map)
        {
            ArgumentNullException.ThrowIfNull(apiResponse);
            ArgumentNullException.ThrowIfNull(map);

            return new McpGatewayContracts.Common.PaginatedCollectionResponse<TDestination>(
                apiResponse.ActualPage,
                apiResponse.TotalPages,
                apiResponse.TotalItems,
                MapIterator(apiResponse.Items, map));
        }

        private static IEnumerable<TDestination> MapIterator<TSource, TDestination>(
            IEnumerable<TSource> source,
            Func<TSource, TDestination> map)
        {
            foreach (var item in source)
            {
                yield return map(item);
            }
        }
    }
}