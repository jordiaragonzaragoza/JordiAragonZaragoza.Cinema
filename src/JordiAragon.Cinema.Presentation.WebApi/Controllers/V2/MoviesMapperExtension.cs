namespace JordiAragon.Cinema.Presentation.WebApi.Controllers.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragon.Cinema.Application.Contracts.Features.Movie.Queries;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Movie.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Mappers.V2;

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