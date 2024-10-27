namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Queries
{
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetMoviesQuery : IQuery<IEnumerable<MovieOutputDto>>;
}