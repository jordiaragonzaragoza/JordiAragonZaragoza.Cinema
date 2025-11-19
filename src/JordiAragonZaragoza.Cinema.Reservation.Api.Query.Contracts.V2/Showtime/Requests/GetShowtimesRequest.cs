namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Requests
{
    using System;

    public sealed record class GetShowtimesRequest(
        Guid? AuditoriumId,
        Guid? MovieId,
        DateTimeOffset? StartTimeOnUtc,
        DateTimeOffset? EndTimeOnUtc,
        string? MovieTitle,
        string? AuditoriumName);
}