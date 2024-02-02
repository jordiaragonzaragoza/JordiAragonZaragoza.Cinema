namespace JordiAragon.Cinema.Reservation.Movie.Application.Contracts.ReadModels
{
    using System;

    public record class MovieReadModel(Guid Id, string Title, TimeSpan Runtime);
}