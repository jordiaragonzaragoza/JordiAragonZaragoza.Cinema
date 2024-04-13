namespace JordiAragon.Cinema.Reservation.Auditorium.Presentation.HttpRestfulApi.V1
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Ticket.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Ticket.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;

    public sealed class AuditoriumsMapper : Profile
    {
        public AuditoriumsMapper()
        {
            // Requests to queries or commands.
            this.CreateMap<ScheduleShowtimeRequest, ScheduleShowtimeCommand>();
            this.CreateMap<ReserveSeatsRequest, ReserveSeatsCommand>();

            // OutputDtos to responses.
            this.CreateMap<SeatOutputDto, SeatResponse>();
            this.CreateMap<Result<IEnumerable<SeatOutputDto>>, Result<IEnumerable<SeatResponse>>>();
            this.CreateMap<AuditoriumOutputDto, AuditoriumResponse>();
            this.CreateMap<Result<IEnumerable<AuditoriumOutputDto>>, Result<IEnumerable<AuditoriumResponse>>>();
            this.CreateMap<TicketOutputDto, TicketResponse>();
            this.CreateMap<Result<TicketOutputDto>, Result<TicketResponse>>();

            this.CreateMap<AvailableSeatReadModel, SeatResponse>()
                .ForCtorParam(nameof(AvailableSeatReadModel.Id), opt => opt.MapFrom(src => src.SeatId));

            this.CreateMap<Result<IEnumerable<AvailableSeatReadModel>>, Result<IEnumerable<SeatResponse>>>();
        }
    }
}