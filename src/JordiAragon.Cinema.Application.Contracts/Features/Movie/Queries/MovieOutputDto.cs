namespace JordiAragon.Cinema.Application.Contracts.Features.Movie.Queries
{
    using System;

    public record class MovieOutputDto(Guid Id, string Title, string ImdbId, string Stars, DateTime ReleaseDateOnUtc);
}