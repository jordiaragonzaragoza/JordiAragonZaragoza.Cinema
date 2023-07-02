namespace JordiAragon.Cinema.Application.Features.Auditorium.Queries
{
    using System;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Queries;
    using JordiAragon.Cinema.Application.Mappers;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;

    public static class AuditoriumMapperExtension
    {
        public static void MapAuditorium(this Mapper maps)
        {
            maps.CreateMap<AuditoriumId, Guid>()
                .ConvertUsing(src => src.Value);

            maps.CreateMap<Auditorium, AuditoriumOutputDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));

            maps.CreateMap<SeatId, Guid>()
                .ConvertUsing(src => src.Value);

            maps.CreateMap<Seat, SeatOutputDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));
        }
    }
}