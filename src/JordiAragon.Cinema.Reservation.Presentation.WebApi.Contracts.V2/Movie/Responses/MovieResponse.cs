namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Movie.Responses
{
    using System;

    public sealed record class MovieResponse(Guid Id, string Title, TimeSpan Runtime);
}