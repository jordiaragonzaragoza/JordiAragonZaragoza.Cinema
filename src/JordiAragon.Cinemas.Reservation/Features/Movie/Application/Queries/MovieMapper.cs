namespace JordiAragon.Cinemas.Reservation.Movie.Application.Queries
{
    using System;
    using AutoMapper;
    using JordiAragon.Cinemas.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragon.Cinemas.Reservation.Movie.Domain;

    public class MovieMapper : Profile
    {
        public MovieMapper()
        {
            this.CreateMap<MovieId, Guid>()
                .ConvertUsing(src => src.Value);

            this.CreateMap<Movie, MovieOutputDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));
        }
    }
}