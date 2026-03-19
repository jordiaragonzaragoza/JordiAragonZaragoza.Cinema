namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Requests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public sealed record class ReserveSeatsBodyRequest(
        [Description("The Ids of the seats to reserve.")]
        IEnumerable<Guid> SeatsIds);
}