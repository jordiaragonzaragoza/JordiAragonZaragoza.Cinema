namespace JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Movie.Responses
{
    using System;

    public record class MovieResponse(Guid Id, string Title, string ImdbId, string Stars, DateTime ReleaseDateOnUtc);
}