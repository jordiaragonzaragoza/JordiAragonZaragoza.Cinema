namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Projectors
{
    using AutoMapper;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;

    public sealed class UserMapper : Profile
    {
        public UserMapper()
        {
            this.CreateMap<User, UserOutputDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));
        }
    }
}