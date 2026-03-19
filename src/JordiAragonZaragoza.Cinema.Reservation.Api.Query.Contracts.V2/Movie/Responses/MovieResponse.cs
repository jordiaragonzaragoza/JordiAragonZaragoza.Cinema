namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Movie.Responses
{
    using System;
    using System.ComponentModel;

    public sealed record class MovieResponse(
        [Description("The unique identifier of the movie.")]
        Guid Id,
        [Description("The title of the movie.")]
        string Title,
        [Description("The runtime of the movie.")]
        TimeSpan Runtime);
}