namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses
{
    using System;

    public sealed record class ShowtimeResponse(Guid Id, string MovieTitle, DateTimeOffset SessionDateOnUtc, Guid AuditoriumId, string AuditoriumName);
}