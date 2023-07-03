namespace JordiAragon.Cinema.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Responses
{
    using System;

    public record class ShowtimeResponse(Guid Id, string MovieTitle, DateTime SessionDateOnUtc, Guid AuditoriumId);
}