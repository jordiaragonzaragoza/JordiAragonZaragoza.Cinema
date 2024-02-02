namespace JordiAragon.Cinema.Reservation.Movie.Application.Contracts.Queries
{
    using System.Collections.Generic;
    using JordiAragon.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetMoviesQuery : IQuery<IEnumerable<MovieReadModel>>;
}