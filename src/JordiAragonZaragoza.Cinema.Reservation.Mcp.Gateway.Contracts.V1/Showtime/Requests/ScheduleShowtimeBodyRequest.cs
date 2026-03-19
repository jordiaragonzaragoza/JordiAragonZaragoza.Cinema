namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Showtime.Requests
{
    using System;
    using System.ComponentModel;

    public sealed record class ScheduleShowtimeBodyRequest(
        [Description("The Id of the auditorium where the showtime will take place.")]
        Guid AuditoriumId,
        [Description("The Id of the movie for which to schedule the showtime.")]
        Guid MovieId,
        [Description("The date and time of the showtime in UTC.")]
        DateTimeOffset SessionDateOnUtc);
}