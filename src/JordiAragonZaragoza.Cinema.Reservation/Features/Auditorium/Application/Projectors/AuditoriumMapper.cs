namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Projectors
{
    using AutoMapper;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;

    public sealed class AuditoriumMapper : Profile
    {
        public AuditoriumMapper()
        {
            this.CreateMap<Auditorium, AuditoriumOutputDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value));
        }
    }
}