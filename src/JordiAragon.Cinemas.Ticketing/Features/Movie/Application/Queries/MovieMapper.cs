namespace JordiAragon.Cinemas.Ticketing.Movie.Application.Queries
{
    using System;
    using AutoMapper;
    using JordiAragon.Cinemas.Ticketing.Movie.Application.Contracts.Queries;
    using JordiAragon.Cinemas.Ticketing.Movie.Domain;

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