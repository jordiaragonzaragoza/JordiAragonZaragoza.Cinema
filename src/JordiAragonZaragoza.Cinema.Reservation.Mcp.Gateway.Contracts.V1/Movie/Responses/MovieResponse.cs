namespace JordiAragonZaragoza.Cinema.Reservation.Mcp.Gateway.Contracts.V1.Movie.Responses
{
    using System;

    public sealed record class MovieResponse(Guid Id, string Title, TimeSpan Runtime);
}