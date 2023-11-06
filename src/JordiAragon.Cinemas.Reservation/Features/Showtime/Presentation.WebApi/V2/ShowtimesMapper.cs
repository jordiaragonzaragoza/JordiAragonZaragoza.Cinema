namespace JordiAragon.Cinemas.Reservation.Showtime.Presentation.WebApi.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Queries;

    public class ShowtimesMapper : Profile
    {
        public ShowtimesMapper()
        {
            // Requests to queries or commands.
            this.CreateMap<ReserveSeatsRequest, ReserveSeatsCommand>();

            // OutputDtos to responses.
            this.CreateMap<TicketOutputDto, TicketResponse>();
            this.CreateMap<Result<TicketOutputDto>, Result<TicketResponse>>();

            this.CreateMap<ShowtimeOutputDto, ShowtimeResponse>();
            this.CreateMap<Result<IEnumerable<ShowtimeOutputDto>>, Result<IEnumerable<ShowtimeResponse>>>();
        }
    }
}