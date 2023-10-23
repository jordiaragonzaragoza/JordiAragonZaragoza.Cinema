namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests
{
    using System;

    public record class GetAvailableSeatsRequest(Guid AuditoriumId, Guid ShowtimeId);
}