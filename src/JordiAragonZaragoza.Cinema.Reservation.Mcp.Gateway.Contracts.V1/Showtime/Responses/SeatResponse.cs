namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses
{
    using System;
    using System.ComponentModel;

    public sealed record class SeatResponse(
        [Description("The Id of the seat.")]
        Guid Id,
        [Description("The row of the seat.")]
        ushort Row,
        [Description("The number of the seat.")]
        ushort SeatNumber);
}