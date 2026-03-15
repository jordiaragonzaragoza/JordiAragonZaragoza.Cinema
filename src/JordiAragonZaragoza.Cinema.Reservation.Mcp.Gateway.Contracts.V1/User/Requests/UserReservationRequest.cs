namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.User.Requests
{
    using System;
    using System.ComponentModel;

    public sealed record class UserReservationRequest(
        [Description("The user identifier.")]
        Guid UserId,
        [Description("The showtime identifier.")]
        Guid ShowtimeId,
        [Description("The reservation identifier.")]
        Guid ReservationId);
}