namespace JordiAragon.Cinema.Application.Features.Movie.Queries
{
    using System;
    using JordiAragon.Cinema.Application.Contracts.Features.Movie.Queries;
    using JordiAragon.Cinema.Application.Mappers;
    using JordiAragon.Cinema.Domain.MovieAggregate;

    public static class MovieMapperExtension
    {
        public static void MapMovie(this Mapper maps)
        {
            maps.CreateMap<MovieId, Guid>()
                .ConvertUsing(src => src.Value);

            maps.CreateMap<Movie, MovieOutputDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));
        }
    }
}