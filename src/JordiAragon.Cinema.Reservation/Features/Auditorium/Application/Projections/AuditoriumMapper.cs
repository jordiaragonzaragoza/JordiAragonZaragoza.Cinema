namespace JordiAragon.Cinema.Reservation.Auditorium.Application.Projections
{
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;

    public class AuditoriumMapper : Profile
    {
        public AuditoriumMapper()
        {
            this.CreateMap<Auditorium, AuditoriumOutputDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));
        }
    }
}