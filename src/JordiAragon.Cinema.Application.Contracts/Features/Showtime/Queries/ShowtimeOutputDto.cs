namespace JordiAragon.Cinema.Application.Contracts.Features.Showtime.Queries
{
    using System;

    public record class ShowtimeOutputDto(Guid Id, string MovieTitle, DateTime SessionDateOnUtc, Guid AuditoriumId);
}