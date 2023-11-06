namespace JordiAragon.Cinemas.Reservation.Showtime.Application.Queries
{
    using System;
    using AutoMapper;
    using JordiAragon.Cinemas.Reservation.Showtime.Domain;

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