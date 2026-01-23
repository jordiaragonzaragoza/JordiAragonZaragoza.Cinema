namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime.Requests
{
    using System;

    public sealed record class ScheduleShowtimeBodyRequest(Guid AuditoriumId, Guid MovieId, DateTimeOffset SessionDateOnUtc);
}