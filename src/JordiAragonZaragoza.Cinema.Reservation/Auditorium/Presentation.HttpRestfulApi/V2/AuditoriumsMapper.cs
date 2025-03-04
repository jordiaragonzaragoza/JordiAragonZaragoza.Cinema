namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Presentation.HttpRestfulApi.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed class AuditoriumsMapper : Profile
    {
        public AuditoriumsMapper()
        {
            // OutputDtos to responses.
            this.CreateMap<SeatOutputDto, SeatResponse>();
            this.CreateMap<Result<IEnumerable<SeatOutputDto>>, Result<IEnumerable<SeatResponse>>>();

            this.CreateMap<AuditoriumReadModel, AuditoriumResponse>();
            this.CreateMap<Result<AuditoriumReadModel>, Result<AuditoriumResponse>>();

            this.CreateMap<PaginatedCollectionOutputDto<AuditoriumReadModel>, PaginatedCollectionResponse<AuditoriumResponse>>();
            this.CreateMap<Result<PaginatedCollectionOutputDto<AuditoriumReadModel>>, Result<PaginatedCollectionResponse<AuditoriumResponse>>>();

            this.CreateMap<SeatReadModel, SeatResponse>();

            this.CreateMap<AvailableSeatReadModel, SeatResponse>()
                .ForCtorParam(nameof(AvailableSeatReadModel.Id), opt => opt.MapFrom(src => src.SeatId));
            this.CreateMap<Result<IEnumerable<AvailableSeatReadModel>>, Result<IEnumerable<SeatResponse>>>();
        }
    }
}