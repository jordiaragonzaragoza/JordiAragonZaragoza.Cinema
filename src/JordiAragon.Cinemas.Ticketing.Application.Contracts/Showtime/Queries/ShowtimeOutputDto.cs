namespace JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Queries
{
    using System;

    public record class ShowtimeOutputDto(Guid Id, string MovieTitle, DateTime SessionDateOnUtc, Guid AuditoriumId);
}