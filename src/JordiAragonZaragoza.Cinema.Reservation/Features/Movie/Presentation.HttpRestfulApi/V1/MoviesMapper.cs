namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Presentation.HttpRestfulApi.V1
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Movie.Responses;

    public sealed class MoviesMapper : Profile
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