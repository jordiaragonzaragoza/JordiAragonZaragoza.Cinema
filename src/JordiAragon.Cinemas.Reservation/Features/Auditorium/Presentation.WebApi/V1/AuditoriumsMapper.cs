namespace JordiAragon.Cinemas.Reservation.Auditorium.Presentation.WebApi.V1
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinemas.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Responses;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Responses;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Ticket.Requests;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Ticket.Responses;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Queries;

    public class AuditoriumsMapper : Profile
    {
        public AuditoriumsMapper()
        {
            // Requests to queries or commands.
            this.CreateMap<CreateShowtimeRequest, CreateShowtimeCommand>();
            this.CreateMap<ReserveSeatsRequest, ReserveSeatsCommand>();

            // OutputDtos to responses.
            this.CreateMap<SeatOutputDto, SeatResponse>();
            this.CreateMap<Result<IEnumerable<SeatOutputDto>>, Result<IEnumerable<SeatResponse>>>();
            this.CreateMap<AuditoriumOutputDto, AuditoriumResponse>();
            this.CreateMap<Result<IEnumerable<AuditoriumOutputDto>>, Result<IEnumerable<AuditoriumResponse>>>();
            this.CreateMap<TicketOutputDto, TicketResponse>();
            this.CreateMap<Result<TicketOutputDto>, Result<TicketResponse>>();

            this.CreateMap<ShowtimeOutputDto, ShowtimeResponse>();
            this.CreateMap<Result<IEnumerable<ShowtimeOutputDto>>, Result<IEnumerable<ShowtimeResponse>>>();
        }
    }
}