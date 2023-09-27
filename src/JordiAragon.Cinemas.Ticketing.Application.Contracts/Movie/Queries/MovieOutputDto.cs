namespace JordiAragon.Cinemas.Ticketing.Application.Contracts.Movie.Queries
{
    using System;

    public record class MovieOutputDto(Guid Id, string Title, string ImdbId, string Stars, DateTime ReleaseDateOnUtc);
}