namespace JordiAragon.Cinema.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests
{
    using System;

    public record class CreateShowtimeRequest(Guid MovieId, DateTime SessionDateOnUtc);
}