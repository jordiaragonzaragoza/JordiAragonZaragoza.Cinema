namespace JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests
{
    using System;

    public sealed record class ScheduleShowtimeRequest(Guid AuditoriumId, Guid MovieId, DateTimeOffset SessionDateOnUtc);
}