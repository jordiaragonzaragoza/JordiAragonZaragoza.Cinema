namespace JordiAragon.Cinemas.Ticketing.Application.Contracts.Movie.Queries
{
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetMoviesQuery : IQuery<IEnumerable<MovieOutputDto>>;
}