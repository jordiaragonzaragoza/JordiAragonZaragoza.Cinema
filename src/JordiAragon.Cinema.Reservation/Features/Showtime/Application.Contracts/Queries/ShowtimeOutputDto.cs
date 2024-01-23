namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries
{
    using System;

    public record class ShowtimeOutputDto(Guid Id, string MovieTitle, DateTimeOffset SessionDateOnUtc, Guid AuditoriumId);
}