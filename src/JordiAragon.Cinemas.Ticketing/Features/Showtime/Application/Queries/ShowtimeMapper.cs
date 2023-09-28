namespace JordiAragon.Cinemas.Ticketing.Showtime.Application.Queries
{
    using System;
    using AutoMapper;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain;

    public class ShowtimeMapper : Profile
    {
        public ShowtimeMapper()
        {
            this.CreateMap<ShowtimeId, Guid>()
                .ConvertUsing(src => src.Value);

            this.CreateMap<TicketId, Guid>()
                .ConvertUsing(src => src.Value);
        }
    }
}