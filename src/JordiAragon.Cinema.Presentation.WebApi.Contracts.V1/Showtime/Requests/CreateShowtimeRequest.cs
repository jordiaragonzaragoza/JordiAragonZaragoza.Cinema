namespace JordiAragon.Cinema.Presentation.WebApi.Contracts.V1.Showtime.Requests
{
    using System;

    public record class CreateShowtimeRequest(Guid AuditoriumId, Guid MovieId, DateTime SessionDateOnUtc);
}