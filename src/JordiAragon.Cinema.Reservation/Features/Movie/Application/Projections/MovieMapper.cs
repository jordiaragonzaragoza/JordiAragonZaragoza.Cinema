namespace JordiAragon.Cinema.Reservation.Movie.Application.Projections
{
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Movie.Domain;

    public class MovieMapper : Profile
    {
        public MovieMapper()
        {
            this.CreateMap<Movie, MovieOutputDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));
        }
    }
}