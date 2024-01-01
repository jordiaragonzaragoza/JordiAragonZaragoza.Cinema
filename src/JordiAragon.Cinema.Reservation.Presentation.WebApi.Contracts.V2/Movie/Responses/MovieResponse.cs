namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Movie.Responses
{
    using System;

    public record class MovieResponse(Guid Id, string Title, TimeSpan Runtime);
}