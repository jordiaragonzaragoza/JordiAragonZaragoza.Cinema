namespace JordiAragon.Cinemas.Reservation.Movie.Application.Contracts.Queries
{
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetMoviesQuery : IQuery<IEnumerable<MovieOutputDto>>;
}