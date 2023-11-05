namespace JordiAragon.Cinemas.Reservation.Movie.Presentation.WebApi.V1
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinemas.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V1.Movie.Responses;

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