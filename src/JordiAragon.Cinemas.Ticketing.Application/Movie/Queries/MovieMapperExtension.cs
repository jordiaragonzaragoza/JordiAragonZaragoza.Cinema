namespace JordiAragon.Cinemas.Ticketing.Application.Movie.Queries
{
    using System;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Movie.Queries;
    using JordiAragon.Cinemas.Ticketing.Domain.MovieAggregate;

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