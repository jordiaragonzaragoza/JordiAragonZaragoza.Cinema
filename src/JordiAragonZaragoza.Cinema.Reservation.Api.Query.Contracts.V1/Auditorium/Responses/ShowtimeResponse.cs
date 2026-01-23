namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1.Auditorium.Responses
{
    using System;

    public sealed record class ShowtimeResponse(Guid Id, string MovieTitle, DateTimeOffset SessionDateOnUtc, Guid AuditoriumId);
}