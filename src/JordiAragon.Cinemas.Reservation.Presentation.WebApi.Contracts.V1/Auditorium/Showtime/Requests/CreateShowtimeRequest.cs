namespace JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests
{
    using System;

    public record class CreateShowtimeRequest(Guid AuditoriumId, Guid MovieId, DateTime SessionDateOnUtc);
}