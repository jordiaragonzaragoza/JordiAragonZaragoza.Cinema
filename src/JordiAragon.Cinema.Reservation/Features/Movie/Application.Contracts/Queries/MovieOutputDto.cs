namespace JordiAragon.Cinema.Reservation.Movie.Application.Contracts.Queries
{
    using System;

    public record class MovieOutputDto(Guid Id, string Title, TimeSpan Runtime);
}