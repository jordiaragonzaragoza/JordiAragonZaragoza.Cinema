namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V1.Movie.Responses
{
    using System;

    public record class MovieResponse(Guid Id, string Title, string ImdbId, string Stars, DateTime ReleaseDateOnUtc);
}