namespace JordiAragon.Cinema.Reservation.Auditorium.Application.Queries
{
    using System;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;

    public class AuditoriumMapper : Profile
    {
        public AuditoriumMapper()
        {
            this.CreateMap<AuditoriumId, Guid>()
                .ConvertUsing(src => src.Value);

            this.CreateMap<Auditorium, AuditoriumOutputDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));

            this.CreateMap<SeatId, Guid>()
                .ConvertUsing(src => src.Value);

            this.CreateMap<Seat, SeatOutputDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));
        }
    }
}