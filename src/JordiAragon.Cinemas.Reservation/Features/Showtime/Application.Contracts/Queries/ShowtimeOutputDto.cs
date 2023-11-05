namespace JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Queries
{
    using System;

    public record class ShowtimeOutputDto(Guid Id, string MovieTitle, DateTime SessionDateOnUtc, Guid AuditoriumId);
}