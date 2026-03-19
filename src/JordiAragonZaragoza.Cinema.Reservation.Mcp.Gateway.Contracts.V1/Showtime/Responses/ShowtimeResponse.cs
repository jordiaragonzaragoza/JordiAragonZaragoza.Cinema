namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Responses
{
    using System;
    using System.ComponentModel;

    public sealed record class ShowtimeResponse(
        [Description("The Id of the showtime.")]
        Guid Id,
        [Description("The title of the movie.")]
        string MovieTitle,
        [Description("The date and time of the session in UTC.")]
        DateTimeOffset SessionDateOnUtc,
        [Description("The Id of the auditorium.")]
        Guid AuditoriumId,
        [Description("The name of the auditorium.")]
        string AuditoriumName);
}