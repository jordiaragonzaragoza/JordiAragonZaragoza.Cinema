namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.User.Requests
{
    using System;
    using System.ComponentModel;

    public sealed record class UserReservationsRequest(
        [Description("The user identifier.")]
        Guid UserId,
        [Description("The showtime identifier.")]
        Guid? ShowtimeId,
        [Description("The start interval time on UTC.")]
        DateTimeOffset? StartIntervalTimeOnUtc,
        [Description("The end interval time on UTC.")]
        DateTimeOffset? EndIntervalTimeOnUtc,
        [Description("The auditorium name.")]
        string? AuditoriumName,
        [Description("The movie title.")]
        string? MovieTitle,
        [Description("Indicates if the reservation is purchased.")]
        bool? IsPurchased);
}