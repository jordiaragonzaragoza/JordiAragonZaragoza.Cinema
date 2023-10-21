namespace JordiAragon.Cinemas.Ticketing.Movie.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinemas.Ticketing.Movie.Application.Contracts.Queries;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Movie.Responses;

    public class MoviesMapper : Profile
    {
        public MoviesMapper()
        {
            // Requests to queries or commands.

            // OutputDtos to responses.
            this.CreateMap<MovieOutputDto, MovieResponse>();
            this.CreateMap<Result<IEnumerable<MovieOutputDto>>, Result<IEnumerable<MovieResponse>>>();
        }
    }
}