namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Controllers.V2
{
    using System.Collections.Generic;
    using Ardalis.Result;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Auditorium.Queries;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Commands;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Mappers.V2;

    public static class AuditoriumsMapperExtension
    {
        public static void MapAuditorium(this Mapper maps)
        {
            // Requests to queries or commands.
            maps.CreateMap<CreateShowtimeRequest, CreateShowtimeCommand>();

            // OutputDtos to responses.
            maps.CreateMap<SeatOutputDto, SeatResponse>();
            maps.CreateMap<Result<IEnumerable<SeatOutputDto>>, Result<IEnumerable<SeatResponse>>>();
            maps.CreateMap<AuditoriumOutputDto, AuditoriumResponse>();
            maps.CreateMap<Result<IEnumerable<AuditoriumOutputDto>>, Result<IEnumerable<AuditoriumResponse>>>();
        }
    }
}