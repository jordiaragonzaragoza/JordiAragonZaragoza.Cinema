namespace JordiAragon.Cinemas.Reservation.Movie.Application.Contracts.Queries
{
    using System;

    public record class MovieOutputDto(Guid Id, string Title, string ImdbId, string Stars, DateTime ReleaseDateOnUtc);
}