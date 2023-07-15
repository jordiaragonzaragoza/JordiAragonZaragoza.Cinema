namespace JordiAragon.Cinema.Application.Features.Showtime.Queries
{
    using System;
    using JordiAragon.Cinema.Application.Mappers;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;

    public static class ShowtimeMapperExtension
    {
        public static void MapShowtime(this Mapper maps)
        {
            maps.CreateMap<ShowtimeId, Guid>()
                .ConvertUsing(src => src.Value);

            maps.CreateMap<TicketId, Guid>()
                .ConvertUsing(src => src.Value);
        }
    }
}