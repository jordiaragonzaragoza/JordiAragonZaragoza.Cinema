namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium.Requests
{
    using System;

    public sealed record class ScheduleShowtimeBodyRequest(Guid MovieId, DateTimeOffset SessionDateOnUtc);
}