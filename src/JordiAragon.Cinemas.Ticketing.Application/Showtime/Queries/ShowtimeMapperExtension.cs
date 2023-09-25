namespace JordiAragon.Cinemas.Ticketing.Application.Showtime.Queries
{
    using System;
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate;

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