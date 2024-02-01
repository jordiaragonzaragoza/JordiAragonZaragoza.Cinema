namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Responses
{
    using System;

    public record class ShowtimeResponse(Guid Id, string MovieTitle, DateTimeOffset SessionDateOnUtc, Guid AuditoriumId, string AuditoriumName);
}