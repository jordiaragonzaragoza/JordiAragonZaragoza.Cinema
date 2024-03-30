namespace JordiAragon.Cinema.Reservation.Auditorium.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;

    public class AuditoriumsMapper : Profile
    {
        public AuditoriumsMapper()
        {
            // OutputDtos to responses.
            this.CreateMap<SeatOutputDto, SeatResponse>();
            this.CreateMap<Result<IEnumerable<SeatOutputDto>>, Result<IEnumerable<SeatResponse>>>();

            this.CreateMap<AuditoriumOutputDto, AuditoriumResponse>();
            this.CreateMap<Result<IEnumerable<AuditoriumOutputDto>>, Result<IEnumerable<AuditoriumResponse>>>();

            this.CreateMap<SeatReadModel, SeatResponse>();

            this.CreateMap<AvailableSeatReadModel, SeatResponse>()
                .ForCtorParam(nameof(AvailableSeatReadModel.Id), opt => opt.MapFrom(src => src.SeatId));
            this.CreateMap<Result<IEnumerable<AvailableSeatReadModel>>, Result<IEnumerable<SeatResponse>>>();
        }
    }
}