namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Common
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public record class PaginatedCollectionResponse<T>(
        [Description("The actual page number.")]
        int ActualPage,
        [Description("The total number of pages.")]
        int TotalPages,
        [Description("The total number of items.")]
        int TotalItems,
        [Description("The collection of items.")]
        IEnumerable<T> Items);
}