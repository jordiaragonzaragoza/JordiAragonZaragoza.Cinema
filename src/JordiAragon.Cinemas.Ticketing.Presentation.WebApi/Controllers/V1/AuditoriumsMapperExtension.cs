namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Controllers.V1
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Auditorium.Queries;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Commands;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Queries;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Responses;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Requests;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Responses;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Ticket.Requests;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1.Auditorium.Showtime.Ticket.Responses;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Mappers.V1;

    public static class AuditoriumsMapperExtension
    {
        public static void MapAuditorium(this Mapper maps)
        {
            // Requests to queries or commands.
            maps.CreateMap<CreateShowtimeRequest, CreateShowtimeCommand>();
            maps.CreateMap<CreateTicketRequest, ReserveSeatsCommand>();

            // OutputDtos to responses.
            maps.CreateMap<SeatOutputDto, SeatResponse>();
            maps.CreateMap<Result<IEnumerable<SeatOutputDto>>, Result<IEnumerable<SeatResponse>>>();
            maps.CreateMap<AuditoriumOutputDto, AuditoriumResponse>();
            maps.CreateMap<Result<IEnumerable<AuditoriumOutputDto>>, Result<IEnumerable<AuditoriumResponse>>>();
            maps.CreateMap<TicketOutputDto, TicketResponse>();
            maps.CreateMap<Result<TicketOutputDto>, Result<TicketResponse>>();

            maps.CreateMap<ShowtimeOutputDto, ShowtimeResponse>();
            maps.CreateMap<Result<IEnumerable<ShowtimeOutputDto>>, Result<IEnumerable<ShowtimeResponse>>>();
        }
    }
}