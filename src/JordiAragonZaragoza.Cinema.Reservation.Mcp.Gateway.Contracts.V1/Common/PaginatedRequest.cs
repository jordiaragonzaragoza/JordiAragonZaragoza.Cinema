namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Common
{
    using System.ComponentModel;

    public record class PaginatedRequest
    {
        [Description("The page number. Default value is 1.")]
        [DefaultValue(1)]
        public int PageNumber { get; init; }

        [Description("The page size. Default value is 10.")]
        [DefaultValue(10)]
        public int PageSize { get; init; }
    }
}