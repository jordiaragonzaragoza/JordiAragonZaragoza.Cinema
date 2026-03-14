namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Requests
{
    using System;
    using System.ComponentModel;

    public sealed record class GetShowtimesRequest(
        [Description("The Id of the auditorium.")]
        Guid? AuditoriumId,
        [Description("The Id of the movie.")]
        Guid? MovieId,
        [Description("The start time of the showtimes in UTC.")]
        DateTimeOffset? StartTimeOnUtc,
        [Description("The end time of the showtimes in UTC.")]
        DateTimeOffset? EndTimeOnUtc,
        [Description("The title of the movie.")]
        string? MovieTitle,
        [Description("The name of the auditorium.")]
        string? AuditoriumName);
}