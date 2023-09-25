namespace JordiAragon.Cinema.Presentation.WebApi.Controllers.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragon.Cinema.Application.Contracts.Showtime.Commands;
    using JordiAragon.Cinema.Application.Contracts.Showtime.Queries;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Mappers.V2;

    public static class ShowtimesMapperExtension
    {
        public static void MapShowtime(this Mapper maps)
        {
            // Requests to queries or commands.
            maps.CreateMap<CreateTicketRequest, ReserveSeatsCommand>();

            // OutputDtos to responses.
            maps.CreateMap<TicketOutputDto, TicketResponse>();
            maps.CreateMap<Result<TicketOutputDto>, Result<TicketResponse>>();

            maps.CreateMap<ShowtimeOutputDto, ShowtimeResponse>();
            maps.CreateMap<Result<IEnumerable<ShowtimeOutputDto>>, Result<IEnumerable<ShowtimeResponse>>>();
        }
    }
}
