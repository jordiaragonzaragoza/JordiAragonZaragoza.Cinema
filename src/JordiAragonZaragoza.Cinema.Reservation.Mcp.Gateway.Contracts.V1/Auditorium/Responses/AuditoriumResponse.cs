namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Auditorium.Responses
{
    using System;
    using System.ComponentModel;

    public sealed record class AuditoriumResponse(
        [Description("The auditorium identifier.")]
        Guid Id,
        [Description("The auditorium name.")]
        string Name);
}