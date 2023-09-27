namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Controllers.V1
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Movie.Queries;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Movie.Responses;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Mappers.V1;

    public static class MoviesMapperExtension
    {
        public static void MapMovie(this Mapper maps)
        {
            // Requests to queries or commands.

            // OutputDtos to responses.
            maps.CreateMap<MovieOutputDto, MovieResponse>();
            maps.CreateMap<Result<IEnumerable<MovieOutputDto>>, Result<IEnumerable<MovieResponse>>>();
        }
    }
}