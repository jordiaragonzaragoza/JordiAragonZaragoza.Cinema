namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.User.Responses
{
    using System;
    using System.ComponentModel;

    public sealed record class UserResponse(
        [Description("The User identifier.")]
        Guid Id);
}