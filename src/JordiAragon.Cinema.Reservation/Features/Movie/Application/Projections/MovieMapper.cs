namespace JordiAragon.Cinema.Reservation.Movie.Application.Projections
{
    using System;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Movie.Domain;

    public class MovieMapper : Profile
    {
        public MovieMapper()
        {
            this.CreateMap<MovieId, Guid>()
                .ConvertUsing(src => src.Value);

            this.CreateMap<Movie, MovieReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));
        }
    }
}