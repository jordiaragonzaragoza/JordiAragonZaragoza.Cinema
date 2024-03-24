namespace JordiAragon.Cinema.Reservation.User.Application.Projections
{
    using System;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.User.Domain;

    public class UserMapper : Profile
    {
        public UserMapper()
        {
            this.CreateMap<UserId, Guid>()
                .ConvertUsing(src => src.Value);

            this.CreateMap<User, UserOutputDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));
        }
    }
}