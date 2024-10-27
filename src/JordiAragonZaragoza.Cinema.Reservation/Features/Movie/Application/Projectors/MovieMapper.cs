namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Projectors
{
    using AutoMapper;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;

    public sealed class MovieMapper : Profile
    {
        public MovieMapper()
        {
            this.CreateMap<Movie, MovieOutputDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));
        }
    }
}