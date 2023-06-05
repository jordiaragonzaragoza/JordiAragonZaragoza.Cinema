namespace JordiAragon.Cinema.Presentation.WebApi.Controllers.V1
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragon.Cinema.Application.Contracts.Features.Movie.Queries;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V1.Movie.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Mappers.V1;

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