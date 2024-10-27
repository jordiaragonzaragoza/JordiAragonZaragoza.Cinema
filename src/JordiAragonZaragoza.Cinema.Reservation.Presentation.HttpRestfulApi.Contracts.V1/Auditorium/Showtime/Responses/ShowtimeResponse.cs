namespace JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Responses
{
    using System;

    public sealed record class ShowtimeResponse(Guid Id, string MovieTitle, DateTimeOffset SessionDateOnUtc, Guid AuditoriumId);
}